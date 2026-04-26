using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity.Base;

namespace PlotCreator.DAL.Repositories
{
    public class EntityRepository<T> : IEntityRepository<T> where T : EntityBase
    {
        private readonly ApplicationDBContext _db;

        public EntityRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task Add(T entity)
        {
            _db.Set<T>().Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<T> Update(T entity)
        {
            _db.Set<T>().Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public T GetOne(int id) =>
            _db.Set<T>().First(e => e.Id == id);

        public IQueryable<T> GetAll() => _db.Set<T>();

        public async Task<IReadOnlyList<T>> GetByWorldIdAsync(int worldId) =>
            await _db.Set<T>()
                .Where(e => e.WorldId == worldId)
                .AsNoTracking()
                .ToListAsync();
    }
}
