using PlotCreator.Domain.Filters;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Service.Interfaces
{
	public interface IFilterService
	{
		Task<IBaseResponse<IEnumerable<CharacterViewModel>>> FilterAllUserCharacters(CharacterFilter characterFilter);
		Task<IBaseResponse<IEnumerable<EventViewModel>>> FilterAllUserEvents(EventFilter eventFilter);
	}
}
