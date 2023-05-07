using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Helpers.SQL.Interfaces
{
	public interface ISQLReaderHelper<T>
	{
		Task<T> ReadUnique();
	}
}
