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
    public interface IEpisodeService : IUserInfo
    {
        Task<IBaseResponse<EpisodeViewModel>> CreateEpisode(EpisodeViewModel model);
        Task<IBaseResponse<bool>> DeleteEpisode(int id);
        Task<IBaseResponse<EpisodeViewModel>> EditEpisode(EpisodeViewModel model);
        Task<IBaseResponse<IEnumerable<EpisodeViewModel>>> GetAllEpisodes(int userId);
        Task<IBaseResponse<IEnumerable<EpisodeViewModel>>> GetBookEpisodes(int bookId);
        Task<IBaseResponse<EpisodeViewModel>> GetEpisode(int episodeId);
    }
}
