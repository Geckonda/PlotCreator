using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces
{
    public interface IEpisodeRepository : IPlotterRepository<Episode>
    {
        Task<IQueryable<Episode>> GetAllByAnotherEntityId(int entityId);
        Task<Book> GetEpisodeBook(int bookId);
    }
}
