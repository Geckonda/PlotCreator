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
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDBContext _db;

        public EventRepository(ApplicationDBContext db)
        {
            _db= db;
        }
        public async Task Add(Event entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Event entity)
        {
            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Event> Update(Event entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public IQueryable<Event> GetAll()
        {
            return _db.Events
                .Include(x => x.Book)
                .Include(x => x.Book!.User);
        }

        public async Task<IQueryable<Event>> GetAllByBookId(int bookId)
        {
            return _db.Events
                .Include(x => x.Book)
                .Include(x => x.Book!.User)
                .Where(x => x.Book!.Id == bookId);
        }

        public async Task<IQueryable<Event>> GetAllByUserId(int userId)
        {
            return _db.Events
                .Include(x => x.Book)
                .Include(x => x.Book!.User)
                .Where(x => x.Book!.User!.Id == userId);
        }

        public Task<Event> GetEmptyViewModel()
        {
            throw new NotImplementedException();
        }

        public async Task<Event> GetOne(int id)
        {
            return _db.Events
                .Where(x => x.Id == id)
                .Include(x => x.Book)
                .Include(x => x.Book!.User)
                .First();
        }

        public async Task<Book> GetBook(int bookId)
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
