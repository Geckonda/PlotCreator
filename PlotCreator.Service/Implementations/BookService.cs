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
        private readonly IPlotterRepository<Book> _bookRepository;
        public BookService(IPlotterRepository<Book> bookRepository)
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
                    Access_ModificatorId = model.Access_Modificator!.Id,
                    Title = model.Title,
                    RatingId = model.Rating!.Id,
                    GenreId = model.Genre!.Id,
                    Book_StatusId = model.Book_Status!.Id,
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
                var book = await _bookRepository.GetOne(id);
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

        public async Task<IBaseResponse<Book>> EditBook(BookViewModel model)
        {
            var baseResponse = new BaseResponse<Book>();
            try
            {
                var book = await _bookRepository.GetOne(model.Id);
				if (book == null)
				{
					baseResponse.Description = "Книга не найдена";
					baseResponse.StatusCode = StatusCode.NotFound;
					return baseResponse;
				}
                book.Title = model.Title;
                book.Description = model.Description;
				book.Access_ModificatorId = model.Access_Modificator!.Id;
                book.RatingId = model.Rating!.Id;
                book.GenreId = model.Genre!.Id;
                book.Book_StatusId = model.Book_Status!.Id;
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
                var book = await _bookRepository.GetOne(id);
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
                var books = await _bookRepository.GetAllByUserId(userId);
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
				var emptyBook = await _bookRepository.GetEmptyViewModel();
                var emptyModel = new BookViewModel()
                {
                    Access_Modificators = emptyBook.Access_Modificators,
                    Ratings = emptyBook.Ratings,
                    Genres = emptyBook.Genres,
                    Book_Statuses = emptyBook.Book_Statuses,
                };
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
