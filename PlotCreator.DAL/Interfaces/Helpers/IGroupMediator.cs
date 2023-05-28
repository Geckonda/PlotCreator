using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces.Helpers
{
	public interface IGroupMediator<T>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="entityId"></param>
		/// <returns>Возвращает все группы, привязанные к конкретной сущности</returns>
		Task<IQueryable<T>> GetEntityGroups(int entityId);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupId"></param>
		/// <param name="entityId"></param>
		/// <returns>Возвращает все группы книги</returns>
		Task<IQueryable<T>> GetAllEntityGroupsByBookId(int bookId);
		Task AddGroupsToEntity(IEnumerable<T> groups);
		Task DeleteGroupsFromEntity(IEnumerable<T> groups);
		Task EditGroupsEntityRelation(IEnumerable<T> groups, int entityId, int bookId);
	}
}
