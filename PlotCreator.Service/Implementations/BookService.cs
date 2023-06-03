using PlotCreator.DAL.Interfaces;
using PlotCreator.DAL.Repositories;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Response.Implementations;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Service.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBaseRepository<Book> _bookRepository;
        public BookService(IBaseRepository<Book> baseRepository) 
        {
            _bookRepository = baseRepository;
        }

        public Task<IBaseResponse<BookViewModel>> CreateBook(BookViewModel book)
        {
            throw new NotImplementedException();
        }

        public Task<IBaseResponse<bool>> DeleteBook(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IBaseResponse<Book>> EditBook(int? id, BookViewModel book)
        {
            throw new NotImplementedException();
        }

        public async Task<IBaseResponse<BookViewModel>> GetBook(int id)
        {
            var baseResponse = new BaseResponse<BookViewModel>();
            try
            {
                var book = _bookRepository.GetOne(id);
                if (book == null)
                {
                    baseResponse.Description = "Не найдено ни одной книги";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                var data = new BookViewModel()
                {/*
                    Id = id,
                    UserId = book.UserId,
                    Access_Modificator = book.Access_Modificator,
                    Access_Modificators = book.Access_Modificators,
                    Title = book.Title,
                    Rating = book.Rating,
                    Ratings = book.Ratings,
                    Genre = book.Genre,
                    Genres = book.Genres,
                    Book_Status = book.Book_Status,
                    Book_Statuses = book.Book_Statuses,
                    Description = book.Description,
                    Book_cover = book.Book_cover,
                    Ideas = book.Ideas,
                    Characters = book.Characters,
                    Episodes = book.Episodes,*/
                };
                baseResponse.Data = data;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<BookViewModel>()
                {
                    Description = $"[BookService | GetBook]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public Task<IBaseResponse<IEnumerable<Book>>> GetBooks()
        {
            throw new NotImplementedException();
        }
    }
}
