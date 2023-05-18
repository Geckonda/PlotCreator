using PlotCreator.DAL.Interfaces.Helpers;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Multiple_Tables;
using PlotCreator.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces
{
    public interface ICharacterRepository : IPlotterRepository<Character>, IBookHelper<Character>, IBookMediator<Book_Character>, IGroupMediator<Group_Character>
	{
        Task<Character> GetEmptyViewModel(int bookId);
		Task<int> GetLastUserCharacterId(int userId);

		//Мульти-запросы
		Task<IQueryable<Character>> GetAllExcludeCurrentBookCharacters(int userId, int bookId);
    }
}
