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
    public interface IIdeaRepository: IBaseRepository<Idea>, IBookEntity<Idea>, IBookMediator<Book_Idea>, IUserEntity<Idea>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="bookId"></param>
        /// <returns>Список идей, которые не привязаны к книге.</returns>
        Task<IQueryable<Idea>> GetAllExcludeCurrentBookIdeas(int userid, int bookId);
    }
}
