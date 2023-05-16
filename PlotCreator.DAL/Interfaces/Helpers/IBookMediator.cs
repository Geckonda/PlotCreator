using PlotCreator.Domain.Entity.Multiple_Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces.Helpers
{
	public interface IBookMediator<T>
	{
		IQueryable<T> GetBookEntitiesRelations(int bookId, int[] entityIds);
		Task AddEntitiesToBook(IEnumerable<T> entities);
		Task DeleteEntitiesFromBook(IEnumerable<T> entities);
	}
}
