using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Interfaces.Helpers
{
	public interface ICharacterMediator<T>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="entityId"></param>
		/// <returns>Персонажи, привязанные к сущности T.</returns>
		Task<IQueryable<T>> GetEntityCharacters(int entityId);
		Task AddCharactersToEntity(IEnumerable<T> characters);
		Task DeleteCharactersFromEntity(IEnumerable<T> characters);
		/// <summary>
		/// Добавляет связи <see cref="Character"/> и T, посступающие в параметре, остальные удаляет.
		/// </summary>
		/// <param name="characters"></param>
		/// <param name="entityId"></param>
		/// <param name="bookId"></param>
		/// <returns></returns>
		Task EditCharactersEntityRelation(IEnumerable<T> characters, int entityId, int bookId);
	}
}
