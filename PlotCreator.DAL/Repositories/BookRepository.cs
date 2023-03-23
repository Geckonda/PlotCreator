using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Repositories
{
    public class BookRepository : IBaseRepository<Book>
    {
        private readonly ApplicationDBContext _db;
        public BookRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> Add(Book entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Book entity)
        {
            _db.Books.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public IQueryable<Book> GetAll()
        {
            return _db.Books;
        }

        public async Task<Book> Update(Book entity)
        {
            _db.Books.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
