using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Repositories
{
    public class EpisodeRepository : IPlotterRepository<Episode>
    {
        private readonly ApplicationDBContext _db;
        public EpisodeRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task Add(Episode entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        public async Task Delete(Episode entity)
        {
            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }
        public async Task<Episode> Update(Episode entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public IQueryable<Episode> GetAll()
        {
            return _db.Episodes
                .Include(x => x.Book);
        }

        public async Task<IQueryable<Episode>> GetAllByAnotherEntityId(int entityId)
        {
            return _db.Episodes
                .Where(x => x.BookId == entityId)
                .Include(x => x.Book);
        }

        public Task<IQueryable<Episode>> GetAllByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<Episode> GetEmptyViewModel()
        {
            throw new NotImplementedException();
        }

        public async Task<Episode> GetOne(int id)
        {
            return _db.Episodes
                .Where(x => x.Id == id)
                .Include(x => x.Book)
                .First();
        }
    }
}
