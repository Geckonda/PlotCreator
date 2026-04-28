using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Repositories
{
    public class EntityTypeRepository : IEntityTypeRepository
    {
        private readonly ApplicationDBContext _db;

        public EntityTypeRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task Add(EntityType entity)
        {
            _db.EntityTypes.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(EntityType entity)
        {
            _db.EntityTypes.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<EntityType> Update(EntityType entity)
        {
            _db.EntityTypes.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public EntityType GetOne(int id) => _db.EntityTypes.First(t => t.Id == id);

        public IQueryable<EntityType> GetAll() => _db.EntityTypes;

        public async Task<IReadOnlyList<EntityType>> GetByOwnerAsync(int ownerUserId) =>
            await _db.EntityTypes
                .Where(t => t.OwnerUserId == ownerUserId)
                .OrderBy(t => t.Id)
                .AsNoTracking()
                .ToListAsync();

        public async Task<EntityType?> GetByOwnerAndKeyAsync(int ownerUserId, string key) =>
            await _db.EntityTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.OwnerUserId == ownerUserId && t.Key == key);

        public async Task<int> CountEntitiesUsingTypeAsync(int typeId) =>
            await _db.Entities.CountAsync(e => e.TypeId == typeId);
    }
}
