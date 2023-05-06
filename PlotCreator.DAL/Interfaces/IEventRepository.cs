using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces
{
    public interface IEventRepository : IPlotterRepository<Event>
    {
        Task<IQueryable<Event>> GetAllByAnotherEntityId(int entityId);
        Task<Book> GetEventBook(int bookId);
    }
}
