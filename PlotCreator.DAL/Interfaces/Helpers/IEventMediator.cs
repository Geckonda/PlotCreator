using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces.Helpers
{
	public interface IEventMediator<T>
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entityId"></param>
		/// <returns>События, привязанные к сущности T.</returns>
		Task<IQueryable<T>> GetEntityEvents(int entityId);
		Task AddEventsToEntity(IEnumerable<T> events);
		Task DeleteEventsFromEntity(IEnumerable<T> events);
		/// <summary>
		/// Добавляет связи <see cref="Event"/> и T, посступающие в параметре, остальные удаляет.
		/// </summary>
		/// <param name="events"></param>
		/// <param name="entityId"></param>
		/// <param name="bookId"></param>
		/// <returns></returns>
		Task EditEventsEntityRelation(IEnumerable<T> events, int entityId, int bookId);
	}
}
