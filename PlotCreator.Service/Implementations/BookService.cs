using PlotCreator.DAL.Interfaces;
using PlotCreator.DAL.Repositories;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Response.Implementations;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Domain.ViewModels.Lists;
using PlotCreator.Service.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Service.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository baseRepository) 
        {
            _bookRepository = baseRepository;
        }

        public async Task<IBaseResponse<BookViewModel>> CreateBook(BookViewModel model)
        {
            var baseResponse = new BaseResponse<BookViewModel>();
            try
            {
                var book = new Book()
                {
                    UserId = model.User!.Id,
                    ModificatorId = model.Modificator!.Id,
                    Title = model.Title,
                    RatingId = model.Rating!.Id,
                    GenreId = model.Genre!.Id,
                    StatusId = model.Status!.Id,
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
                    ErrorForUser = "Не удалось добавить книгу"
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteBook(int id)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var book = _bookRepository.GetOne(id);
                if (book == null)
                {
                    baseResponse.ErrorForUser = "Не удалось найти книгу";
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
                    ErrorForUser = "Не удалось удалить книгу"
                };
            }
        }

        public async Task<IBaseResponse<BookViewModel>> EditBook(BookViewModel model)
        {
            var baseResponse = new BaseResponse<BookViewModel>();
            try
            {
                var book =  _bookRepository.GetOne(model.Id);
                if (book == null)
                {
                    baseResponse.Description = "Книга не найдена";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                book.Title = model.Title;
                book.Description = model.Description;
                book.ModificatorId = model.Modificator!.Id;
                book.RatingId = model.Rating!.Id;
                book.GenreId = model.Genre!.Id;
                book.StatusId = model.Status!.Id;
                book.Book_cover = model.Book_cover;

                await _bookRepository.Update(book);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<BookViewModel>()
                {
                    Description = $"[BookService | EditBook]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                    ErrorForUser = "Не удалось отредактировать книгу",
                };
            }
        }

        public async Task<IBaseResponse<BookViewModel>> GetBook(int id)
        {
            var baseResponse = new BaseResponse<BookViewModel>();
            try
            {
                var book =  _bookRepository.GetOne(id);
                if (book == null)
                {
                    baseResponse.Description = "Не найдено ни одной книги";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                var lists = await _bookRepository.GetReferenceData();
                var data = GetModelFromBook(book, lists);
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
                    ErrorForUser = "Не удалось открыть книгу",
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<BookViewModel>>> GetBooks(int userId)
        {
            var baseResponse = new BaseResponse<IEnumerable<BookViewModel>>();
            try
            {
                var books = await _bookRepository.GetAllByUserId(userId);
                var data = new List<BookViewModel>();
                foreach(var book in books)
                {
                    var model = GetModelFromBook(book, null);
                    data.Add(model);
                }
                baseResponse.Data = data;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<BookViewModel>>()
                {
                    Description = $"[BookService | GetBooks]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<BookViewModel>> GetLastUserBook(int userId)
        {
            var baseResponse = new BaseResponse<BookViewModel>();
            try
            {
                var id = await _bookRepository.GetLastUserEntityId(userId);
                var book = _bookRepository.GetOne(id);
                var model = GetModelFromBook(book, null);
                baseResponse.Data = model;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<BookViewModel>()
                {
                    Description = $"[BookService | GetLastUserBook]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                    ErrorForUser = "Последняя книга не найдена",
                };
            }
        }

        public async Task<IBaseResponse<BookViewModel>> GetViewModel()
        {
            var baseResponse = new BaseResponse<BookViewModel>();
            try
            {
                var lists = await _bookRepository.GetReferenceData();
                var emptyModel = new BookViewModel()
                {
                    ModificatorList = lists.ModificatorList,
                    GenreList = lists.GenreList,
                    RatingList = lists.RatingList,
                    StatusList = lists.StatusList,
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
                    ErrorForUser = "Внутренняя ошибка",
                };
            }
        }

        public async Task<User> GetUser(int bookId)
        {
            try
            {
                return await _bookRepository.GetUser(bookId);
            }
            catch (Exception)
            {
                return null!;
            }
        }
        public async Task<int> GetUserId(int bookId)
        {
            try
            {
                return await _bookRepository.GetUserId(bookId);
            }
            catch (Exception)
            {

                return (int)StatusCode.NotFound;
            }
        }

        /// <summary>
        /// Конвертирует объект <see cref="Book" /> в объект <see cref="BookViewModel"/>
        /// </summary>
        /// <param name="book"></param>
        /// <param name="lists">Список справочников</param>
        /// <returns></returns>
        private static BookViewModel GetModelFromBook(Book book, BookLists? lists)
        {
            return new BookViewModel()
            {
                Id = book.Id,
                User = book.User,
                Modificator = book.Modificator,
                Title = book.Title,
                Rating = book.Rating,
                Genre = book.Genre,
                Status = book.Status,
                Description = book.Description,
                Book_cover = book.Book_cover,
                Characters = book.Books_Characters
                                 .Select(b_ch => b_ch.Character!)
                                 .ToList(),
                Ideas = book.Books_Ideas
                            .Select(b_idea => b_idea.Idea!)
                            .ToList(),
                Episodes = book.Episodes,
                Events = book.Events,
                ModificatorList = lists?.ModificatorList,
                RatingList = lists?.RatingList,
                GenreList = lists?.GenreList,
                StatusList = lists?.StatusList,
            };
        }

    }
}
