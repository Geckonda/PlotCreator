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
	public interface ICharacterRepository:
		IBaseRepository<Character>,
		IBookEntity<Character>,
		IUserEntity<Character>,
		IViewModel<CharacterLists>,
		IBookMediator<Book_Character>,
		IGroupMediator<Group_Character>,
		IEventMediator<Event_Character>,
		IEpisodeMediator<Episode_Character>
	{

		//Мульти-запросы
		Task<IQueryable<Character>> GetAllExcludeCurrentBookCharacters(int userId, int bookId);
	}
}
