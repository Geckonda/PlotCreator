using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Repositories
{
    public class GraphNodeRepository : IGraphNodeRepository
    {
        private readonly ApplicationDBContext _db;

        public GraphNodeRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task Add(GraphNode entity)
        {
            _db.GraphNodes.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(GraphNode entity)
        {
            _db.GraphNodes.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<GraphNode> Update(GraphNode entity)
        {
            _db.GraphNodes.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public GraphNode GetOne(int id) => _db.GraphNodes.First(n => n.Id == id);

        public IQueryable<GraphNode> GetAll() => _db.GraphNodes;

        public async Task<GraphNode?> FindAsync(int nodeId) =>
            await _db.GraphNodes.FirstOrDefaultAsync(n => n.Id == nodeId);

        public async Task<bool> ExistsAsync(int graphId, int entityId) =>
            await _db.GraphNodes.AnyAsync(n => n.GraphId == graphId && n.EntityId == entityId);

        public async Task RemoveWithAttachedEdgesAsync(int nodeId)
        {
            await _db.GraphEdges
                .Where(e => e.FromNodeId == nodeId || e.ToNodeId == nodeId)
                .ExecuteDeleteAsync();
            await _db.GraphNodes
                .Where(n => n.Id == nodeId)
                .ExecuteDeleteAsync();
        }
    }
}
