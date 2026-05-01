using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Repositories
{
    public class EntityRepository : IEntityRepository
    {
        private readonly ApplicationDBContext _db;

        public EntityRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task Add(WorldEntity entity)
        {
            _db.Entities.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(WorldEntity entity)
        {
            _db.Entities.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<WorldEntity> Update(WorldEntity entity)
        {
            _db.Entities.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public WorldEntity GetOne(int id) => _db.Entities.First(e => e.Id == id);

        public IQueryable<WorldEntity> GetAll() => _db.Entities;

        public async Task<IReadOnlyList<WorldEntity>> GetByWorldIdAsync(int worldId) =>
            await _db.Entities
                .Include(e => e.Type)
                .Where(e => e.WorldId == worldId)
                .AsNoTracking()
                .ToListAsync();

        public async Task<WorldEntity?> GetWithTypeAsync(int id) =>
            await _db.Entities
                .Include(e => e.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
    }
}
