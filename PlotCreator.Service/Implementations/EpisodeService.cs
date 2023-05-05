using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Response.Implementations;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Service.Implementations
{
    public class EpisodeService : IEpisodeService
    {
        private readonly IPlotterRepository<Episode> _episodeRepository;
        public EpisodeService(IPlotterRepository<Episode> episodeRepository)
        {
            _episodeRepository = episodeRepository;
        }

        public async Task<IBaseResponse<EpisodeViewModel>> CreateEpisode(EpisodeViewModel model)
        {
            var baseResponse = new BaseResponse<EpisodeViewModel>();
            try
            {
                var episode = new Episode()
                {
                    BookId = model.Book!.Id,
                    Heading = model.Heading,
                    Position = model.Position,
                    Content = model.Content,
                };
                await _episodeRepository.Add(episode);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<EpisodeViewModel>()
                {
                    Description = $"[EpisodeService | CreateEpisode]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteEpisode(int id)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var episode = await _episodeRepository.GetOne(id);
                if (episode == null)
                {
                    baseResponse.Description = "Эпизод не найден";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                await _episodeRepository.Delete(episode);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[EpisodeService | DeleteEpisode]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<EpisodeViewModel>> EditEpisode(EpisodeViewModel model)
        {
            var baseResponse = new BaseResponse<EpisodeViewModel>();
            try
            {
                var episode = await _episodeRepository.GetOne(model.Id);
                if (episode == null)
                {
                    baseResponse.Description = "Эпизод не найден";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                episode.Heading = model.Heading;
                episode.Position = model.Position;
                episode.Content = model.Content;
                await _episodeRepository.Update(episode);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<EpisodeViewModel>()
                {
                    Description = $"[EpisodeService | EditEpisode]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<EpisodeViewModel>>> GetAllEpisodes(int userId)
        {
            var baseResponse = new BaseResponse<IEnumerable<EpisodeViewModel>>();
            try
            {
                var episodes = await _episodeRepository.GetAllByUserId(userId);
                List<EpisodeViewModel> episodeModels = new List<EpisodeViewModel>();
                foreach (var episode in episodes)
                {
                    var model = new EpisodeViewModel()
                    {
                        Id = episode.Id,
                        Book = episode.Book,
                        Heading = episode.Heading,
                        Position= episode.Position,
                        Content = episode.Content,
                    };
                    episodeModels.Add(model);
                }
                baseResponse.Data = episodeModels;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<EpisodeViewModel>>()
                {
                    Description = $"[EpisodeService | GetAllEpisodes]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<EpisodeViewModel>>> GetBookEpisodes(int bookId)
        {
            var baseResponse = new BaseResponse<IEnumerable<EpisodeViewModel>>();
            try
            {
                var episodes = await _episodeRepository.GetAllByAnotherEntityId(bookId);
                List<EpisodeViewModel> episodeModels = new List<EpisodeViewModel>();
                foreach (var episode in episodes)
                {
                    var model = new EpisodeViewModel()
                    {
                        Id = episode.Id,
                        Book = episode.Book,
                        Heading = episode.Heading,
                        Position = episode.Position,
                        Content = episode.Content,
                    };
                    episodeModels.Add(model);
                }
                baseResponse.Data = episodeModels;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<EpisodeViewModel>>()
                {
                    Description = $"[EpisodeService | GetBookEpisodes]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<EpisodeViewModel>> GetEpisode(int episodeId)
        {
            var baseResponse = new BaseResponse<EpisodeViewModel>();
            try
            {
                var episode = await _episodeRepository.GetOne(episodeId);
                if (episode == null)
                {
                    baseResponse.Description = "Эпизод не найден";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                var data = new EpisodeViewModel()
                {
                    Id = episode.Id,
                    Book = episode.Book,
                    Heading = episode.Heading,
                    Position = episode.Position,
                    Content = episode.Content,
                };
                baseResponse.Data = data;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<EpisodeViewModel>()
                {
                    Description = $"[EpisodeService | GetBookEpisodes]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public Task<int> GetUserId(int contentId)
        {
            throw new NotImplementedException();
        }
    }
}
