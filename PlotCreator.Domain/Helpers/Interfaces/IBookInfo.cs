using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Helpers.Interfaces
{
    public interface IBookInfo
    {
        Task<int> GetBookId(int contentId);
        Task<Book> GetBook(int entityId);
    }
}
