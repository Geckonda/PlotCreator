using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;

namespace PlotCreator.DAL.Repositories
{
    public class RelationRepository : IRelationRepository
    {
        private readonly ApplicationDBContext _db;

        public RelationRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task Add(Relation entity)
        {
            _db.Relations.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Relation entity)
        {
            _db.Relations.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Relation> Update(Relation entity)
        {
            _db.Relations.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public Relation GetOne(int id) => _db.Relations.First(r => r.Id == id);

        public IQueryable<Relation> GetAll() => _db.Relations;

        public async Task<IReadOnlyList<Relation>> GetByWorldIdAsync(int worldId) =>
            await _db.Relations
                .Where(r => r.WorldId == worldId)
                .AsNoTracking()
                .ToListAsync();

        public async Task DeleteForEntityAsync(int entityId, EntityType type)
        {
            await _db.Relations
                .Where(r => (r.FromId == entityId && r.FromType == type) ||
                            (r.ToId == entityId && r.ToType == type))
                .ExecuteDeleteAsync();
        }
    }
}
