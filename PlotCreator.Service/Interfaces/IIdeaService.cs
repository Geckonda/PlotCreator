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
    public interface IIdeaService : IUserInfo, IBookInfo
    {
        Task<IBaseResponse<IEnumerable<IdeaViewModel>>> GetIdeas(int userId);
        Task<IBaseResponse<IEnumerable<IdeaViewModel>>> GetBookIdeas(int bookId);
        Task<IBaseResponse<IdeaViewModel>> GetIdea(int id);
        Task<IBaseResponse<IdeaViewModel>> CreateIdea(IdeaViewModel ideaViewModel);
        Task<IBaseResponse<IdeaViewModel>> EditIdea(int id, IdeaViewModel ideaViewModel);
        Task<IBaseResponse<bool>> DeleteIdea(int id);

        //
        Task<IBaseResponse<IdeaViewModel>> GetLastUserIdea(int userId);
        //Совмещение таблиц
        Task<IBaseResponse<IEnumerable<IdeaViewModel>>> GetIdeaExcludeBook(int userId, int bookId);
        Task<IBaseResponse<bool>> AddIdeasToBook(int bookId, int[] ideaIds);
        Task<IBaseResponse<bool>> DeleteIdeasFromBook(int bookId, int[] ideaIds);
    }
}
