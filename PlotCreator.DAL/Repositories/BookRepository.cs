using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public IQueryable<Book> GetAll(int parentId)
        {

            var Books = (from book in _db.Books
                        join genre in _db.Genres on book.GenreId equals genre.Id
                        join status in _db.Statuses on book.Book_StatusId equals status.Id
                        join modificator in _db.Modificators on book.Access_ModificatorId equals modificator.Id
                        join rating in _db.Ratings on book.RatingId equals rating.Id
                        where book.UserId == parentId
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
                            Book_cover = book.Book_cover,
                            Ideas = (from b in _db.Books
                                     join b_i in _db.Books_Ideas on b.Id equals b_i.BookId
                                     join i in _db.Ideas on b_i.IdeaId equals i.Id
                                     where i.UserId == book.UserId && b.Id == book.Id
                                     select new Idea
                                     {
                                         Id = i.Id,
                                         UserId = i.UserId,
                                         Topic = i.Topic,
                                         Data_Creation = i.Data_Creation,
                                         Content = i.Content,
                                     }).ToList(),
                            Episodes = (from b in _db.Books
                                        join e in _db.Episodes on b.Id equals e.BookId
                                        where b.Id == book.Id
                                        select new Episode
                                        {
                                            Id = e.Id,
                                            BookId = e.BookId,
                                            Heading = e.Heading,
                                            Content = e.Content,

                                        }).ToList(),
                        });
            return Books;
        }

        public IQueryable<Book> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Book> Update(Book entity)
        {
            _db.Books.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        private List<Idea> GetIdeasForBook(int bookId)
        {
            return (from book in _db.Books
                    join b_i in _db.Books_Ideas on book.Id equals b_i.BookId
                    join idea in _db.Ideas on b_i.IdeaId equals idea.Id
                    where book.Id == bookId
                    select new Idea
                    {
                        Id = idea.Id,
                        UserId = idea.UserId,
                        Topic = idea.Topic,
                        Data_Creation = idea.Data_Creation,
                        Content = idea.Content,
                    }).ToList();
        }
        private List<Episode> GetEpisodesForBook(int bookId)
        {
            return (from b in _db.Books
                    join e in _db.Episodes on b.Id equals e.BookId
                    where b.Id == bookId
                    select new Episode
                    {
                        Id = e.Id,
                        BookId = e.BookId,
                        Heading = e.Heading,
                        Content = e.Content,

                    }).ToList();
        }
    }
}
