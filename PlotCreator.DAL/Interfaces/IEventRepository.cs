using PlotCreator.DAL.Interfaces.Helpers;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Multiple_Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces
{
    public interface IEventRepository : IPlotterRepository<Event>, IBookHelper<Event>, IGroupMediator<Group_Event>
    {
        Task<Event> GetEmptyViewModel(int bookId);
        Task<int> GetLastUserEventId(int bookId);
    }
}
