using PlotCreator.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces
{
    public interface ICharacterRepository<Character> : IBaseRepository<Character>
    {
        Task<Character> GetOne(int id);
        Task<CharacterViewModel> GetEmptyViewModel();
        Task<IQueryable<Character>> GetAllCharacters(int userId);
        Task<IQueryable<Character>> GetBookCharacters(int bookId);
    }
}
