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
            var Books = from book in _db.Books
                        join genre in _db.Genres on book.GenreId equals genre.Id
                        join status in _db.Statuses on book.Book_StatusId equals status.Id
                        join modificator in _db.Modificators on book.Access_ModificatorId equals modificator.Id
                        join rating in _db.Ratings on book.RatingId equals rating.Id
                        select new Book
                        {
                            Id = book.Id,
                            UserId= book.UserId,
                            Title= book.Title,
                            Access_ModificatorId = modificator.Id,
                            
                            Modificator = modificator.Modificator,
                            RatingId = rating.Id,
                            Rate = rating.Rate,
                            GenreId = genre.Id,
                            GenreString = genre.Name,
                            Book_StatusId = status.Id,
                            Status = status.Status,
                            Description = book.Description,
                            Book_cover = book.Book_cover
                        };
            return (IQueryable<Book>)Books;

        }

        public async Task<Book> Update(Book entity)
        {
            _db.Books.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
