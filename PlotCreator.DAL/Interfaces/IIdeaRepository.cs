using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Multiple_Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces
{
    public interface IIdeaRepository : IPlotterRepository<Idea>
    {
        Task<IQueryable<Idea>> GetAllByBookId(int bookId);
        Task<int> GetLastUserIdeaId(int userId);

        //Мульти-запросы
        Task<IQueryable<Idea>> GetAllExcludeCurrentBookIdeas(int userid, int bookId);
        IQueryable<Book_Idea> GetBookIdeasRelations(int bookId, int[] ideaIds);
        Task AddIdeasToBook(IEnumerable<Book_Idea> entities);
        Task DeleteIdeasFromBook(IEnumerable<Book_Idea> entities);
    }
}
