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
        public BookService(IBaseRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
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

        public Task<IBaseResponse<BookViewModel>> GetBook(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IBaseResponse<IEnumerable<Book>>> GetBooks(int userId)
        {
            var baseResponse = new BaseResponse<IEnumerable<Book>>();
            try
            {
                var books = _bookRepository.GetAll()
                    .Where(x => x.UserId == userId);
                if(!books.Any())
                {
                    baseResponse.Description= "Не найдено ни одной книги";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                baseResponse.Data = books;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Book>>()
                {
                    Description = $"[BookService | GetBooks]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }

        }

        public Task<int> GetUserId(int bookId)
        {
            throw new NotImplementedException();
        }
    }
}
