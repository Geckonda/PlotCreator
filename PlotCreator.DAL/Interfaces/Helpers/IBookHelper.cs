using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces.Helpers
{
    public interface IBookHelper<T>
    {
		Task<IQueryable<T>> GetAllByBookId(int bookId);
		Task<Book> GetBook(int bookId);
	}
}
