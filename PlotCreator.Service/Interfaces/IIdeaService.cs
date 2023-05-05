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
    public interface IIdeaService : IUserInfo
    {

        Task<IBaseResponse<IEnumerable<Idea>>> GetIdeas(int userId);
        Task<IBaseResponse<IdeaViewModel>> GetIdea(int id);
        Task<IBaseResponse<IdeaViewModel>> CreateIdea(IdeaViewModel ideaViewModel);
        Task<IBaseResponse<Idea>> EditIdea(int? id, IdeaViewModel ideaViewModel);
        Task<IBaseResponse<bool>> DeleteIdea(int id);
    }
}
