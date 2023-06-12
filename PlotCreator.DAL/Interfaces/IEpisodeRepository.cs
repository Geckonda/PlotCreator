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
	public interface IEpisodeRepository:
		IBaseRepository<Episode>,
		IBookEntity<Episode>,
		IUserEntity<Episode>,
		IEventMediator<Episode_Event>,
		ICharacterMediator<Episode_Character>,
		IViewModel<EpisodeLists>
	{
	}
}
