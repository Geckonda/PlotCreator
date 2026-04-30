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
    public class RelationService : IRelationService
    {
        private readonly IRelationRepository _relations;
        private readonly ApplicationDBContext _db;

        public RelationService(IRelationRepository relations, ApplicationDBContext db)
        {
            _relations = relations;
            _db = db;
        }

        public async Task<IBaseResponse<IReadOnlyList<RelationDto>>> GetByWorldAsync(int worldId)
        {
            var rels = await _relations.GetByWorldIdAsync(worldId);
            return Ok<IReadOnlyList<RelationDto>>(rels.Select(ToDto).ToList());
        }

        public async Task<IBaseResponse<RelationDto>> CreateAsync(int worldId, RelationCreateRequest request)
        {
            if (!await EntityExistsInWorldAsync(worldId, request.FromId))
                return NotFound<RelationDto>("From entity not found in this world");
            if (!await EntityExistsInWorldAsync(worldId, request.ToId))
                return NotFound<RelationDto>("To entity not found in this world");

            var rel = new Relation
            {
                WorldId = worldId,
                FromId = request.FromId,
                ToId = request.ToId,
                Label = NormalizeLabel(request.Label)
            };
            await _relations.Add(rel);
            return Ok(ToDto(rel));
        }

        public async Task<IBaseResponse<RelationDto>> UpdateAsync(int id, RelationUpdateRequest request)
        {
            var rel = await _relations.GetAll().FirstOrDefaultAsync(r => r.Id == id);
            if (rel is null) return NotFound<RelationDto>("Relation not found");
            rel.Label = NormalizeLabel(request.Label);
            await _relations.Update(rel);
            return Ok(ToDto(rel));
        }

        public async Task<IBaseResponse<bool>> DeleteAsync(int id)
        {
            var rel = await _relations.GetAll().FirstOrDefaultAsync(r => r.Id == id);
            if (rel is null) return NotFound<bool>("Relation not found");
            await _relations.Delete(rel);
            return Ok(true);
        }

        private async Task<bool> EntityExistsInWorldAsync(int worldId, int entityId) =>
            await _db.Entities.AnyAsync(e => e.Id == entityId && e.WorldId == worldId);

        private static string? NormalizeLabel(string? label)
        {
            if (string.IsNullOrWhiteSpace(label)) return null;
            return label.Trim();
        }

        private static RelationDto ToDto(Relation r) => new()
        {
            Id = r.Id,
            WorldId = r.WorldId,
            FromId = r.FromId,
            ToId = r.ToId,
            Label = r.Label,
            CreatedAt = r.CreatedAt
        };

        private static IBaseResponse<T> Ok<T>(T data) =>
            new BaseResponse<T> { Data = data, StatusCode = StatusCode.Ok };

        private static IBaseResponse<T> NotFound<T>(string desc) =>
            new BaseResponse<T> { StatusCode = StatusCode.NotFound, Description = desc, ErrorForUser = desc };
    }
}
