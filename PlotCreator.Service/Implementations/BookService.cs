using Microsoft.EntityFrameworkCore;
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
using static System.Reflection.Metadata.BlobBuilder;

namespace PlotCreator.Service.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository<Book> _bookRepository;
        public BookService(IBookRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<IBaseResponse<BookViewModel>> CreateBook(BookViewModel model)
        {
            var baseResponse = new BaseResponse<BookViewModel>();
            try
            {
                var book = new Book()
                {
                    UserId = model.UserId,
                    Access_ModificatorId = model.Access_ModificatorId,
                    Title = model.Title,
                    RatingId = model.RatingId,
                    GenreId = model.GenreId,
                    Book_StatusId = model.Book_StatusId,
                    Description = model.Description,
                    Book_cover = model.Book_cover,
                };
                await _bookRepository.Add(book);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<BookViewModel>()
                {
                    Description = $"[BookService | CreateBook]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteBook(int id)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var book = await _bookRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if(book == null)
                {
                    baseResponse.Description = "Книга не найдена";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                await _bookRepository.Delete(book);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[BookService | DeleteBook]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<Book>> EditBook(int? id, BookViewModel model)
        {
            var baseResponse = new BaseResponse<Book>();
            try
            {
                var book = await _bookRepository.GetAll(model.UserId)
                    .FirstOrDefaultAsync(x => x.Id ==id);
				if (book == null)
				{
					baseResponse.Description = "Книга не найдена";
					baseResponse.StatusCode = StatusCode.NotFound;
					return baseResponse;
				}
                book.Title = model.Title;
                book.Description = model.Description;
				book.Access_ModificatorId = model.Access_ModificatorId;
                book.RatingId = model.RatingId;
                book.GenreId = model.GenreId;
                book.Book_StatusId = model.Book_StatusId;
                book.Book_cover = model.Book_cover;

                await _bookRepository.Update(book);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
			}
            catch (Exception ex)
            {
				return new BaseResponse<Book>()
				{
					Description = $"[BookService | EditBook]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
				};
			}
        }

        public async Task<IBaseResponse<BookViewModel>> GetBook(int id)
        {
            var baseResponse = new BaseResponse<BookViewModel>();
            try
            {
                var book = await _bookRepository.GetBookViewModel(id);
                if (book == null)
                {
                    baseResponse.Description = "Не найдено ни одной книги";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                var data = new BookViewModel()
                {
                    Id = id,
                    UserId = book.UserId,
                    Access_ModificatorId = book.Access_ModificatorId,
                    Access_Modificator = book.Access_Modificator,
                    Access_Modificators = book.Access_Modificators,
                    Title = book.Title,
                    RatingId = book.RatingId,
                    Rating = book.Rating,
                    Ratings = book.Ratings,
                    GenreId = book.GenreId,
                    Genre = book.Genre,
                    Genres= book.Genres,
                    Book_StatusId = book.Book_StatusId,
                    Book_Status = book.Book_Status,
                    Book_Statuses = book.Book_Statuses,
                    Description = book.Description,
                    Book_cover = book.Book_cover,
                    Ideas = book.Ideas,
                    Characters = book.Characters,
                    Episodes = book.Episodes,
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

        public async Task<IBaseResponse<IEnumerable<Book>>> GetBooks(int userId)
        {
            var baseResponse = new BaseResponse<IEnumerable<Book>>();
            try
            {
                var books = _bookRepository.GetAll(userId);
                //if(!books.Any())
                //{
                //    baseResponse.Description= "Не найдено ни одной книги";
                //    baseResponse.StatusCode = StatusCode.NotFound;
                //    return baseResponse;
                //}
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

        public async Task<int> GetUserId(int bookId)
        {
            try
            {
                var book = await _bookRepository.GetAll()
                        .FirstOrDefaultAsync(x => x.Id == bookId);
                return book.UserId;
            }
            catch (Exception)
            {
                return 0;
            }
        }

		public async Task<IBaseResponse<BookViewModel>> GetViewModel()
		{
			var baseResponse = new BaseResponse<BookViewModel>();
			try
			{
				var emptyModel = await _bookRepository.GetEmptyBookViewModel();
				baseResponse.Data = emptyModel;
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<BookViewModel>()
				{
					Description = $"[BookService | GetViewModel]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
				};
			}
		}
	}
}
