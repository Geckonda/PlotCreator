using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces.Helpers
{
	public interface IEpisodeMediator<T>
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entityId"></param>
		/// <returns>Эпизоды, привязанные к сущности T.</returns>
		Task<IQueryable<T>> GetEntityEpisodes(int entityId);
		Task AddEpisodesToEntity(IEnumerable<T> episodes);
		Task DeleteEpisodesFromEntity(IEnumerable<T> episodes);
		/// <summary>
		/// Добавляет связи <see cref="Episode"/> и T, посступающие в параметре, остальные удаляет.
		/// </summary>
		/// <param name="episodes"></param>
		/// <param name="entityId"></param>
		/// <param name="bookId"></param>
		/// <returns></returns>
		Task EditEpisodesEntityRelation(IEnumerable<T> episodes, int entityId, int bookId);
	}
}
