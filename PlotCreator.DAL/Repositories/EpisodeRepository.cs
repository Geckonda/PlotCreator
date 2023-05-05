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
    public class EpisodeRepository : IEpisodeRepository
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
                .Include(x => x.Book)
                .Include(x => x.Book!.User);
        }

        public async Task<IQueryable<Episode>> GetAllByAnotherEntityId(int entityId)
        {
            return _db.Episodes
                .Where(x => x.BookId == entityId)
                .Include(x => x.Book)
                .Include(x => x.Book!.User);
        }

        public async Task<IQueryable<Episode>> GetAllByUserId(int userId)
        {
            return _db.Episodes
                .Include(x =>x.Book)
                .Include(x => x.Book!.User)
                .Where(x => x.Book!.User!.Id == userId);
        }

        public async Task<Episode> GetOne(int id)
        {
            return _db.Episodes
                .Where(x => x.Id == id)
                .Include(x => x.Book)
                .Include(x => x.Book!.User)
                .First();
        }

        public async Task<Book> GetEpisodeBook(int bookId)
        {
            return _db.Books
                .Where(x => x.Id == bookId)
                .Include(x => x.User)
                .Include(x => x.Access_Modificator)
                .Include(x => x.Rating)
                .Include(x => x.Genre)
                .Include(x => x.Book_Status)
                .First();
        }
    }
}
