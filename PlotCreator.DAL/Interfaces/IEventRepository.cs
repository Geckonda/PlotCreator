using PlotCreator.DAL.Interfaces.Helpers;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Multiple_Tables;
using PlotCreator.Domain.ViewModels.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces
{
	public interface IEventRepository:
		IBaseRepository<Event>,
		IBookEntity<Event>,
		IUserEntity<Event>,
		IGroupMediator<Group_Event>,
		IEpisodeMediator<Episode_Event>,
		ICharacterMediator<Event_Character>,
		IViewModel<EventLists>
	{
	}
}
