using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
		public async Task<Book> Update(Book entity)
		{
			_db.Books.Update(entity);
			await _db.SaveChangesAsync();
			return entity;
		}

        public IQueryable<Book> GetAll()
        {
			var books = _db.Books
			   .Include(x => x.Access_Modificator)
			   .Include(x => x.Rating)
			   .Include(x => x.Genre)
			   .Include(x => x.Book_Status);
            //Ideas & Episodes
			return books;
		}


		public async Task<Book> GetOne(int id)
		{
            var book =  _db.Books.Where(x => x.Id == id)
                .Include(x => x.Access_Modificator)
                .Include(x => x.Rating)
                .Include(x => x.Genre)
                .Include(x => x.Book_Status)
                .FirstOrDefault();
            book.Access_Modificators = _db.Modificators.ToList();
            book.Ratings = _db.Ratings.ToList();
            book.Genres = _db.Genres.ToList();
            book.Book_Statuses = _db.Statuses.ToList();
            return book;
		}

		public async Task<Book> GetEmptyViewModel()
		{
            return new Book()
            {
                Access_Modificators = _db.Modificators.ToList(),
                Ratings = _db.Ratings.ToList(),
                Genres = _db.Genres.ToList(),
                Book_Statuses = _db.Statuses.ToList(),
            };
		}

		public async Task<IQueryable<Book>> GetAllByUserId(int userId)
		{
            var books = _db.Books.Where(x => x.UserId == userId)
                .Include(x => x.Access_Modificator)
                .Include(x => x.Rating)
                .Include(x => x.Genre)
                .Include(x => x.Book_Status);
			return books;
		}

		public Task<IQueryable<Book>> GetAllByAnotherEntityId(int entityId)
		{
			throw new NotImplementedException();
		}
	}
}
