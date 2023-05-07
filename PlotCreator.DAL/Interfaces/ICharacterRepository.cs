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
    public interface ICharacterRepository : IPlotterRepository<Character>
    {
        Task<IQueryable<Character>> GetAllByAnotherEntityId(int entityId);
        Task<IQueryable<Character>> GetAllExcludeCurrentBookCharacters(int userId, int bookId);
        Task<Character> GetEmptyViewModel();
        Task<int> GetLastUserCharacterId(int userId);
        Task AddCharactersToBook(Book_Character entities);
    }
}
