using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Response.Implementations;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Service.Implementations
{
    public class GraphService : IGraphService
    {
        private readonly IGraphRepository _graphs;
        private readonly IGraphNodeRepository _nodes;
        private readonly IGraphEdgeRepository _edges;
        private readonly ApplicationDBContext _db;
        private readonly ICurrentUserService _currentUser;

        public GraphService(
            IGraphRepository graphs,
            IGraphNodeRepository nodes,
            IGraphEdgeRepository edges,
            ApplicationDBContext db,
            ICurrentUserService currentUser)
        {
            _graphs = graphs;
            _nodes = nodes;
            _edges = edges;
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<IBaseResponse<IReadOnlyList<GraphSummaryDto>>> GetByWorldAsync(int worldId)
        {
            var userId = _currentUser.GetUserId();
            if (!await _db.Worlds.AnyAsync(w => w.Id == worldId))
                return NotFound<IReadOnlyList<GraphSummaryDto>>("World not found");

            await _graphs.EnsureSystemDefaultAsync(worldId, userId);
            var list = await _graphs.GetByWorldAndUserAsync(worldId, userId);
            return Ok<IReadOnlyList<GraphSummaryDto>>(list.Select(ToSummary).ToList());
        }

        public async Task<IBaseResponse<GraphDetailDto>> GetAsync(int graphId)
        {
            var userId = _currentUser.GetUserId();
            var graph = await _graphs.GetWithContentsAsync(graphId);
            if (graph is null) return NotFound<GraphDetailDto>("Graph not found");
            if (graph.OwnerUserId != userId) return Forbidden<GraphDetailDto>("Not your graph");

            if (graph.IsSystemDefault)
                return Ok(await BuildSmartGraphDetailAsync(graph));

            return Ok(ToDetail(graph));
        }

        public async Task<IBaseResponse<GraphSummaryDto>> CreateAsync(int worldId, GraphCreateRequest request)
        {
            var userId = _currentUser.GetUserId();
            if (!await _db.Worlds.AnyAsync(w => w.Id == worldId))
                return NotFound<GraphSummaryDto>("World not found");
            if (string.IsNullOrWhiteSpace(request.Name))
                return Conflict<GraphSummaryDto>("Name is required");

            var graph = new Graph
            {
                WorldId = worldId,
                OwnerUserId = userId,
                Name = request.Name.Trim(),
                Description = request.Description,
                IsSystemDefault = false,
                Kind = "Custom"
            };
            await _graphs.Add(graph);
            return Ok(ToSummary(graph));
        }

        public async Task<IBaseResponse<GraphSummaryDto>> UpdateAsync(int graphId, GraphUpdateRequest request)
        {
            var userId = _currentUser.GetUserId();
            var graph = _db.Graphs.FirstOrDefault(g => g.Id == graphId);
            if (graph is null) return NotFound<GraphSummaryDto>("Graph not found");
            if (graph.OwnerUserId != userId) return Forbidden<GraphSummaryDto>("Not your graph");
            if (graph.IsSystemDefault) return Forbidden<GraphSummaryDto>("Default graph is read-only");

            if (request.Name is not null)
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                    return Conflict<GraphSummaryDto>("Name cannot be empty");
                graph.Name = request.Name.Trim();
            }
            if (request.Description is not null) graph.Description = request.Description;

            await _graphs.Update(graph);
            return Ok(ToSummary(graph));
        }

        public async Task<IBaseResponse<bool>> DeleteAsync(int graphId)
        {
            var userId = _currentUser.GetUserId();
            var graph = _db.Graphs.FirstOrDefault(g => g.Id == graphId);
            if (graph is null) return NotFound<bool>("Graph not found");
            if (graph.OwnerUserId != userId) return Forbidden<bool>("Not your graph");
            if (graph.IsSystemDefault) return Forbidden<bool>("Default graph cannot be deleted");

            await _graphs.Delete(graph);
            return Ok(true);
        }

        public async Task<IBaseResponse<GraphNodeDto>> AddNodeAsync(int graphId, GraphNodeCreateRequest request)
        {
            var guard = await GuardEditableAsync(graphId);
            if (guard is not null) return Map<GraphNodeDto>(guard);

            var graph = (await _graphs.GetSummaryAsync(graphId))!;

            var entity = await _db.Entities.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.EntityId && e.WorldId == graph.WorldId);
            if (entity is null) return NotFound<GraphNodeDto>("Entity not found in this world");

            if (await _nodes.ExistsAsync(graphId, request.EntityId))
                return Conflict<GraphNodeDto>("Entity is already on this graph");

            var node = new GraphNode
            {
                GraphId = graphId,
                EntityId = request.EntityId,
                X = request.X,
                Y = request.Y
            };
            await _nodes.Add(node);

            var dto = new GraphNodeDto
            {
                Id = node.Id,
                EntityId = entity.Id,
                EntityName = entity.Name,
                EntityTypeKey = await ResolveTypeKeyAsync(entity.TypeId),
                X = node.X,
                Y = node.Y
            };
            return Ok(dto);
        }

        public async Task<IBaseResponse<GraphNodeDto>> UpdateNodePositionAsync(int graphId, int nodeId, GraphNodePositionRequest request)
        {
            var guard = await GuardEditableAsync(graphId);
            if (guard is not null) return Map<GraphNodeDto>(guard);

            var node = await _db.GraphNodes
                .Include(n => n.Entity!).ThenInclude(e => e.Type)
                .FirstOrDefaultAsync(n => n.Id == nodeId && n.GraphId == graphId);
            if (node is null) return NotFound<GraphNodeDto>("Node not found on this graph");

            node.X = request.X;
            node.Y = request.Y;
            await _nodes.Update(node);

            return Ok(new GraphNodeDto
            {
                Id = node.Id,
                EntityId = node.EntityId,
                EntityName = node.Entity?.Name ?? string.Empty,
                EntityTypeKey = node.Entity?.Type?.Key,
                X = node.X,
                Y = node.Y
            });
        }

        public async Task<IBaseResponse<bool>> RemoveNodeAsync(int graphId, int nodeId)
        {
            var guard = await GuardEditableAsync(graphId);
            if (guard is not null) return Map<bool>(guard);

            var exists = await _db.GraphNodes.AnyAsync(n => n.Id == nodeId && n.GraphId == graphId);
            if (!exists) return NotFound<bool>("Node not found on this graph");

            await _nodes.RemoveWithAttachedEdgesAsync(nodeId);
            return Ok(true);
        }

        public async Task<IBaseResponse<GraphEdgeDto>> AddEdgeAsync(int graphId, GraphEdgeCreateRequest request)
        {
            var guard = await GuardEditableAsync(graphId);
            if (guard is not null) return Map<GraphEdgeDto>(guard);

            if (request.FromNodeId == request.ToNodeId)
                return Conflict<GraphEdgeDto>("Edge endpoints must differ");

            var nodes = await _db.GraphNodes
                .Where(n => n.GraphId == graphId
                            && (n.Id == request.FromNodeId || n.Id == request.ToNodeId))
                .Select(n => n.Id)
                .ToListAsync();
            if (nodes.Count < 2)
                return NotFound<GraphEdgeDto>("Both endpoints must be nodes on this graph");

            var edge = new GraphEdge
            {
                GraphId = graphId,
                FromNodeId = request.FromNodeId,
                ToNodeId = request.ToNodeId,
                Direction = request.Direction,
                Label = request.Label
            };
            await _edges.Add(edge);

            return Ok(ToEdgeDto(edge));
        }

        public async Task<IBaseResponse<GraphEdgeDto>> UpdateEdgeAsync(int graphId, int edgeId, GraphEdgeUpdateRequest request)
        {
            var guard = await GuardEditableAsync(graphId);
            if (guard is not null) return Map<GraphEdgeDto>(guard);

            var edge = await _db.GraphEdges
                .FirstOrDefaultAsync(e => e.Id == edgeId && e.GraphId == graphId);
            if (edge is null) return NotFound<GraphEdgeDto>("Edge not found on this graph");

            if (request.Direction.HasValue) edge.Direction = request.Direction.Value;
            if (request.Label is not null) edge.Label = string.IsNullOrWhiteSpace(request.Label) ? null : request.Label;

            await _edges.Update(edge);
            return Ok(ToEdgeDto(edge));
        }

        public async Task<IBaseResponse<bool>> RemoveEdgeAsync(int graphId, int edgeId)
        {
            var guard = await GuardEditableAsync(graphId);
            if (guard is not null) return Map<bool>(guard);

            var edge = await _db.GraphEdges
                .FirstOrDefaultAsync(e => e.Id == edgeId && e.GraphId == graphId);
            if (edge is null) return NotFound<bool>("Edge not found on this graph");

            await _edges.Delete(edge);
            return Ok(true);
        }

        private async Task<IBaseResponse<object>?> GuardEditableAsync(int graphId)
        {
            var userId = _currentUser.GetUserId();
            var graph = await _graphs.GetSummaryAsync(graphId);
            if (graph is null) return NotFound<object>("Graph not found");
            if (graph.OwnerUserId != userId) return Forbidden<object>("Not your graph");
            if (graph.IsSystemDefault) return Forbidden<object>("Default graph is read-only");
            return null;
        }

        private async Task<GraphDetailDto> BuildSmartGraphDetailAsync(Graph graph)
        {
            var entities = await _db.Entities.AsNoTracking()
                .Include(e => e.Type)
                .Where(e => e.WorldId == graph.WorldId)
                .ToListAsync();

            var relations = await _db.Relations.AsNoTracking()
                .Where(r => r.WorldId == graph.WorldId)
                .ToListAsync();

            var nodeDtos = new List<GraphNodeDto>(entities.Count);
            var idToVirtual = new Dictionary<int, int>();
            var n = entities.Count;
            const double radius = 320.0;
            for (var i = 0; i < n; i++)
            {
                var e = entities[i];
                var theta = n == 0 ? 0 : (2 * System.Math.PI * i / n);
                var virtualId = -(i + 1);
                idToVirtual[e.Id] = virtualId;
                nodeDtos.Add(new GraphNodeDto
                {
                    Id = virtualId,
                    EntityId = e.Id,
                    EntityName = e.Name,
                    EntityTypeKey = e.Type?.Key,
                    X = radius * System.Math.Cos(theta),
                    Y = radius * System.Math.Sin(theta)
                });
            }

            var edgeDtos = new List<GraphEdgeDto>();
            var virtualEdgeId = -1;
            foreach (var r in relations)
            {
                if (!idToVirtual.TryGetValue(r.FromId, out var fromVid)) continue;
                if (!idToVirtual.TryGetValue(r.ToId, out var toVid)) continue;
                edgeDtos.Add(new GraphEdgeDto
                {
                    Id = virtualEdgeId--,
                    FromNodeId = fromVid,
                    ToNodeId = toVid,
                    Direction = EdgeDirection.None,
                    Label = r.Label
                });
            }

            return new GraphDetailDto
            {
                Id = graph.Id,
                WorldId = graph.WorldId,
                OwnerUserId = graph.OwnerUserId,
                Name = graph.Name,
                Description = graph.Description,
                IsSystemDefault = true,
                Kind = graph.Kind,
                Nodes = nodeDtos,
                Edges = edgeDtos
            };
        }

        private async Task<string?> ResolveTypeKeyAsync(int typeId) =>
            await _db.EntityTypes.AsNoTracking()
                .Where(t => t.Id == typeId)
                .Select(t => t.Key)
                .FirstOrDefaultAsync();

        private static GraphSummaryDto ToSummary(Graph g) => new()
        {
            Id = g.Id,
            WorldId = g.WorldId,
            OwnerUserId = g.OwnerUserId,
            Name = g.Name,
            Description = g.Description,
            IsSystemDefault = g.IsSystemDefault,
            Kind = g.Kind,
            CreatedAt = g.CreatedAt,
            UpdatedAt = g.UpdatedAt
        };

        private static GraphDetailDto ToDetail(Graph g) => new()
        {
            Id = g.Id,
            WorldId = g.WorldId,
            OwnerUserId = g.OwnerUserId,
            Name = g.Name,
            Description = g.Description,
            IsSystemDefault = g.IsSystemDefault,
            Kind = g.Kind,
            Nodes = g.Nodes.Select(n => new GraphNodeDto
            {
                Id = n.Id,
                EntityId = n.EntityId,
                EntityName = n.Entity?.Name ?? string.Empty,
                EntityTypeKey = n.Entity?.Type?.Key,
                X = n.X,
                Y = n.Y
            }).ToList(),
            Edges = g.Edges.Select(ToEdgeDto).ToList()
        };

        private static GraphEdgeDto ToEdgeDto(GraphEdge e) => new()
        {
            Id = e.Id,
            FromNodeId = e.FromNodeId,
            ToNodeId = e.ToNodeId,
            Direction = e.Direction,
            Label = e.Label
        };

        private static IBaseResponse<T> Ok<T>(T data) =>
            new BaseResponse<T> { Data = data, StatusCode = StatusCode.Ok };

        private static IBaseResponse<T> NotFound<T>(string desc) =>
            new BaseResponse<T> { StatusCode = StatusCode.NotFound, Description = desc, ErrorForUser = desc };

        private static IBaseResponse<T> Forbidden<T>(string desc) =>
            new BaseResponse<T> { StatusCode = StatusCode.Forbidden, Description = desc, ErrorForUser = desc };

        private static IBaseResponse<T> Conflict<T>(string desc) =>
            new BaseResponse<T> { StatusCode = StatusCode.Conflict, Description = desc, ErrorForUser = desc };

        private static IBaseResponse<T> Map<T>(IBaseResponse<object> src) =>
            new BaseResponse<T>
            {
                StatusCode = src.StatusCode,
                Description = src.Description,
                ErrorForUser = src.ErrorForUser
            };
    }
}
