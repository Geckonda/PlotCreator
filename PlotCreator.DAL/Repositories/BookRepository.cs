using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.ViewModels.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDBContext _db;
        public BookRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task Add(Book entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Book entity)
        {
            _db.Books.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Book> GetAll()
        {
            return _db.Books;
        }

        public async Task<IQueryable<Book>> GetAllByUserId(int userId)
        {
            return _db.Books
                .Where(x => x.UserId == userId);
        }

        public async Task<BookLists> GetReferenceData(int? entityParam = 0)
        {
            return new BookLists
            {
                ModificatorList = _db.Modificators.ToList(),
                RatingList = _db.Ratings.ToList(),
                GenreList = _db.Genres.ToList(),
                StatusList = _db.Statuses.ToList(),
            };
        }

        public async Task<int> GetLastUserEntityId(int userId)
        {
            return _db.Books
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .Select(x => x.Id)
                .First();
        }

        public Book GetOne(int id)
		{
            return _db.Books
                .Include(book => book.Books_Characters)
                    .ThenInclude(b_ch => b_ch.Character)
                .Include(book => book.Books_Ideas)
                    .ThenInclude(b_idea => b_idea.Idea)
                .Include(book => book.Episodes)
                .Include(book => book.Events)
                .Include(book => book.Groups)
                .Where(book => book.Id == id)
                .First();
		}

        public async Task<User> GetUser(int bookId)
        {
            return _db.Books
                .Where(book => book.Id == bookId)
                .Select(book => book.User!)
                .First();
        }
        public async Task<int> GetUserId(int bookId)
        {
            return _db.Books
                .Where(book => book.Id == bookId)
                .Select(book => book.UserId)
                .First();
        }

        public async Task<Book> Update(Book entity)
        {
            _db.Books.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }
}
