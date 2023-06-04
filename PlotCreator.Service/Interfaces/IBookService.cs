using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Helpers.Interfaces;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Service.Interfaces
{
    public interface IBookService: IUserInfo
    {
        Task<IBaseResponse<IEnumerable<BookViewModel>>> GetBooks(int userId);
        Task<IBaseResponse<BookViewModel>> GetViewModel();
        Task<IBaseResponse<BookViewModel>> GetBook(int id);
        Task<User> GetUser(int bookId);
        Task<IBaseResponse<BookViewModel>> GetLastUserBook(int userId);
        Task<IBaseResponse<BookViewModel>> CreateBook(BookViewModel model);
        Task<IBaseResponse<BookViewModel>> EditBook (BookViewModel model);
        Task<IBaseResponse<bool>> DeleteBook (int id);
    }
}
