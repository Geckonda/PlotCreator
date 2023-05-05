using PlotCreator.Domain.Entity;
using PlotCreator.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces
{
    public interface IBookRepository : IPlotterRepository<Book>
    {
        Task<IQueryable<Book>> GetAllByAnotherEntityId(int entityId);
        Task<Book> GetEmptyViewModel();
    }
}
