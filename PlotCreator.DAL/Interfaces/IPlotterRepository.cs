using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces
{
	public interface IPlotterRepository<T> : IBaseRepository<T>
	{
		Task<T> GetOne(int id);
		Task<IQueryable<T>> GetAllByUserId(int userId);
	}
}
