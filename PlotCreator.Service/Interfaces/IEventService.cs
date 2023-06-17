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
	public interface IEventService : IUserInfo, IBookInfo
	{
		Task<IBaseResponse<EventViewModel>> CreateEvent(EventViewModel model);
		Task<IBaseResponse<bool>> DeleteEvent(int id);
		Task<IBaseResponse<EventViewModel>> EditEvent(EventViewModel model);
		Task<IBaseResponse<IEnumerable<EventViewModel>>> GetAllEvents(int userId);
		Task<IBaseResponse<IEnumerable<EventViewModel>>> GetBookEvents(int bookId);
		Task<IBaseResponse<EventViewModel>> GetEvent(int EventId);
		Task<IBaseResponse<EventViewModel>> GetEmptyViewModel(int bookId);

		Task<int> GetLastUserEventId(int userId);

		//Совмещение таблиц
		////С персонажами
		Task<bool> AddCharactersEventRelation(int eventId, int[] characterIds);
		Task<bool> DeleteCharactersEventRelation(int eventId, int[] characterIds);
		Task<bool> EditCharactersEventRelation(int eventId, int[] characterIds, int bookId);
		////С эпизодами
		Task<bool> AddEpisodesEventRelation(int eventId, int[] episodeIds);
		Task<bool> DeleteEpisodesEventRelation(int eventId, int[] episodeIds);
		Task<bool> EditEpisodesEventRelation(int eventId, int[] episodeIds, int bookId);
		////С группами
		Task<bool> AddGroupsEventRelation(int eventId, int[] groupIds);
		Task<bool> DeleteGroupsEventRelation(int eventId, int[] groupIds);
		Task<bool> EditGroupsEventRelation(int eventId, int[] groupIds, int bookId);
	}
}
