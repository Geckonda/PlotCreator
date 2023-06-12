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
	public interface IEpisodeService: IUserInfo, IBookInfo
	{
		Task<IBaseResponse<EpisodeViewModel>> CreateEpisode(EpisodeViewModel model);
		Task<IBaseResponse<bool>> DeleteEpisode(int id);
		Task<IBaseResponse<EpisodeViewModel>> EditEpisode(EpisodeViewModel model);
		Task<IBaseResponse<IEnumerable<EpisodeViewModel>>> GetAllEpisodes(int userId);
		Task<IBaseResponse<IEnumerable<EpisodeViewModel>>> GetBookEpisodes(int bookId);
		Task<IBaseResponse<EpisodeViewModel>> GetEpisode(int episodeId);
		Task<IBaseResponse<EpisodeViewModel>> GetEmptyViewModel(int bookId);

		//
		Task<int> GetLastUserEpisodeId(int userId);
		//Совмещение таблиц
		////С событиями
		Task<bool> AddEventsEpisodeRelation(int episodeId, int[] eventIds);
		Task<bool> DeleteEventsEpisodeRelation(int episodeId, int[] eventIds);
		Task<bool> EditEventsEpisodeRelation(int episodeId, int[] eventIds, int bookId);
		////С персонажами
		Task<bool> AddCharactersEpisodeRelation(int episodeId, int[] characterIds);
		Task<bool> DeleteCharactersEpisodeRelation(int episodeId, int[] characterIds);
		Task<bool> EditCharactersEpisodeRelation(int episodeId, int[] characterIds, int bookId);
	}
}
