using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Helpers.Interfaces;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Service.Interfaces
{
    public interface ICharacterService : IUserInfo
    {
        Task<IBaseResponse<IEnumerable<CharacterViewModel>>> GetAllCharacters(int userId);
        Task<IBaseResponse<IEnumerable<CharacterViewModel>>> GetBookCharacters(int bookId);
        Task<IBaseResponse<CharacterViewModel>> GetCharacter(int id);
        Task<IBaseResponse<CharacterViewModel>> GetEmptyViewModel();
        Task<IBaseResponse<CharacterViewModel>> CreateCharacter(CharacterViewModel model);
        Task<IBaseResponse<CharacterViewModel>> EditCharacter(CharacterViewModel model);
        Task<IBaseResponse<bool>> DeleteCharacter(int id);

        //
        Task<int> GetLastUserCharacterId(int userId);
        //Совмещение таблиц
        Task<IBaseResponse<IEnumerable<CharacterViewModel>>> GetCharacterExcludeBook(int userId, int bookId);
        Task<IBaseResponse<bool>> AddCharactersToBook(int bookId, int[] characterIds);
    }
}
