using PlotCreator.Domain.Entity;
using PlotCreator.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces
{
    public interface IBookRepository<Book> : IBaseRepository<Book>
    {
        Task<BookViewModel> GetBookViewModel(int id);
        Task<BookViewModel> GetEmptyBookViewModel();
    }
}
