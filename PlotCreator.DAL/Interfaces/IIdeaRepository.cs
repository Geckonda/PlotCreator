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
    public interface IIdeaRepository : IPlotterRepository<Idea>, IBookHelper<Idea>, IBookMediator<Book_Idea>
    {
        Task<int> GetLastUserIdeaId(int userId);

        //Мульти-запросы
        Task<IQueryable<Idea>> GetAllExcludeCurrentBookIdeas(int userid, int bookId);
    }
}
