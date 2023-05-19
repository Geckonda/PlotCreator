using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Service.Interfaces
{
    public interface IGroupService
    {
        Task<IBaseResponse<IEnumerable<Group>>> GetBookGroups(int bookId);
        Task<IBaseResponse<IEnumerable<GroupViewModel>>> GetAllGroupsByParent(int bookId, string parent);
        Task<IBaseResponse<GroupViewModel>> CreateCharacterGroup(GroupViewModel model);
        Task<IBaseResponse<bool>> DeleteCharacterGroup(int id);
        Task<IBaseResponse<GroupViewModel>> EditCharacterGroup(GroupViewModel model);

    }
}
