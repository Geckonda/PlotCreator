using Microsoft.Extensions.Logging;
using PlotCreator.DAL.Interfaces;
using PlotCreator.DAL.Repositories;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Multiple_Tables;
using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Response.Implementations;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Domain.ViewModels.Lists;
using PlotCreator.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Service.Implementations
{
	public class EpisodeService: IEpisodeService
	{
		private readonly IEpisodeRepository _episodeRepository;
		public EpisodeService(IEpisodeRepository episodeRepository)
		{
			_episodeRepository = episodeRepository;
		}

		public async Task<bool> AddCharactersEpisodeRelation(int episodeId, int[] characterIds)
		{
			try
			{
				if (characterIds == null)
					return false;

				List<Episode_Character> mediators = new();
				for (int i = 0; i < characterIds.Length; i++)
				{
					var mediator = new Episode_Character()
					{
						EpisodeId = episodeId,
						CharacterId = characterIds[i],
					};
					mediators.Add(mediator);
				}
				await _episodeRepository.AddCharactersToEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> AddEventsEpisodeRelation(int episodeId, int[] eventIds)
		{
			try
			{
				if (eventIds == null)
					return false;

				List<Episode_Event> mediators = new();
				for (int i = 0; i < eventIds.Length; i++)
				{
					var mediator = new Episode_Event()
					{
						EpisodeId = episodeId,
						EventId = eventIds[i],
					};
					mediators.Add(mediator);
				}
				await _episodeRepository.AddEventsToEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<IBaseResponse<EpisodeViewModel>> CreateEpisode(EpisodeViewModel model)
		{
			var baseResponse = new BaseResponse<EpisodeViewModel>();
			try
			{
				var episode = GetEpisodeFromModel(new(), model);
				await _episodeRepository.Add(episode);
				baseResponse.Data = model;
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

		public async Task<bool> DeleteCharactersEpisodeRelation(int episodeId, int[] characterIds)
		{
			try
			{
				if (characterIds == null)
					return false;

				List<Episode_Character> mediators = new();
				for (int i = 0; i < characterIds.Length; i++)
				{
					var mediator = new Episode_Character()
					{
						EpisodeId = episodeId,
						CharacterId = characterIds[i],
					};
					mediators.Add(mediator);
				}
				await _episodeRepository.DeleteCharactersFromEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<IBaseResponse<bool>> DeleteEpisode(int id)
		{
			var baseResponse = new BaseResponse<bool>();
			try
			{
				var episode = _episodeRepository.GetOne(id);
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

		public async Task<bool> DeleteEventsEpisodeRelation(int episodeId, int[] eventIds)
		{
			try
			{
				if (eventIds == null)
					return false;

				List<Episode_Event> mediators = new();
				for (int i = 0; i < eventIds.Length; i++)
				{
					var mediator = new Episode_Event()
					{
						EpisodeId = episodeId,
						EventId = eventIds[i],
					};
					mediators.Add(mediator);
				}
				await _episodeRepository.DeleteEventsFromEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> EditCharactersEpisodeRelation(int episodeId, int[] characterIds, int bookId)
		{
			try
			{

				List<Episode_Character> mediators = new();
				for (int i = 0; i < characterIds.Length; i++)
				{
					var mediator = new Episode_Character()
					{
						EpisodeId = episodeId,
						CharacterId = characterIds[i],
					};
					mediators.Add(mediator);
				}
				await _episodeRepository.EditCharactersEntityRelation(mediators, episodeId, bookId);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<IBaseResponse<EpisodeViewModel>> EditEpisode(EpisodeViewModel model)
		{
			var baseResponse = new BaseResponse<EpisodeViewModel>();
			try
			{
				var episode = _episodeRepository.GetOne(model.Id);
				if (episode == null)
				{
					baseResponse.Description = "Эпизод не найден";
					baseResponse.StatusCode = StatusCode.NotFound;
					return baseResponse;
				}
				episode = GetEpisodeFromModel(episode, model);
				await _episodeRepository.Update(episode);
				baseResponse.Data = model;
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

		public async Task<bool> EditEventsEpisodeRelation(int episodeId, int[] eventIds, int bookId)
		{
			try
			{
				if (eventIds == null)
					return false;

				List<Episode_Event> mediators = new();
				for (int i = 0; i < eventIds.Length; i++)
				{
					var mediator = new Episode_Event()
					{
						EpisodeId = episodeId,
						EventId = eventIds[i],
					};
					mediators.Add(mediator);
				}
				await _episodeRepository.EditEventsEntityRelation(mediators, episodeId, bookId);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public Task<IBaseResponse<IEnumerable<EpisodeViewModel>>> GetAllEpisodes(int userId)
		{
			throw new NotImplementedException();
		}

		public async Task<IBaseResponse<IEnumerable<EpisodeViewModel>>> GetBookEpisodes(int bookId)
		{
			var baseResponse = new BaseResponse<IEnumerable<EpisodeViewModel>>();
			try
			{
				var episodes = await _episodeRepository.GetAllByBookId(bookId);
                var lists = await _episodeRepository.GetReferenceData(bookId);
                List<EpisodeViewModel> episodeModels = new List<EpisodeViewModel>();
				foreach (var episode in episodes)
				{
					var model = GetModelFromEpisode(episode, lists);
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

		public async Task<IBaseResponse<EpisodeViewModel>> GetEmptyViewModel(int bookId)
		{
			var baseResponse = new BaseResponse<EpisodeViewModel>();
			try
			{
				var book = await _episodeRepository.GetBook(bookId);
				var emptyModel = new EpisodeViewModel()
				{
					Book = book,
				};
				baseResponse.Data = emptyModel;
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<EpisodeViewModel>()
				{
					Description = $"[EpisodeService | GetEmptyViewModel]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
				};
			}
		}

		public async Task<IBaseResponse<EpisodeViewModel>> GetEpisode(int episodeId)
		{
			var baseResponse = new BaseResponse<EpisodeViewModel>();
			try
			{
				var episode = _episodeRepository.GetOne(episodeId);
				var lists = await _episodeRepository.GetReferenceData(episode.Book!.Id);
				if (episode == null)
				{
					baseResponse.Description = "Эпизод не найден";
					baseResponse.StatusCode = StatusCode.NotFound;
					return baseResponse;
				}
				var model = GetModelFromEpisode(episode, lists);
				baseResponse.Data = model;
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

		public async Task<int> GetLastUserEpisodeId(int userId)
		{
			try
			{
				return await _episodeRepository.GetLastUserEntityId(userId);
			}
			catch (Exception)
			{
				return (int)StatusCode.NotFound * -1;
			}
		}

		public async Task<int> GetUserId(int episodeId)
		{
			try
			{
				return await _episodeRepository.GetUserId(episodeId);
			}
			catch (Exception)
			{
				return (int)StatusCode.NotFound * -1;
			}
		}

		public async Task<User> GetUser(int episodeId)
		{
			try
			{
				return await _episodeRepository.GetUser(episodeId);
			}
			catch (Exception)
			{
				return null!;
			}
		}

		public Task<int> GetBookId(int contentId)
		{
			throw new NotImplementedException();
		}

		public async Task<Book> GetBook(int bookId)
		{
			try
			{
				return await _episodeRepository.GetBook(bookId);
			}
			catch (Exception)
			{
				return null!;
			}
		}
		//

		private static EpisodeViewModel GetModelFromEpisode(Episode episode, EpisodeLists lists)
		{
			return new EpisodeViewModel()
			{
				Id = episode.Id,
				Book = episode.Book,
				Heading = episode.Heading,
				Position = episode.Position,
				Content = episode.Content,
				Characters = episode.Episodes_Characters
									.Select(ep_ch => ep_ch.Character!)
									.ToList(),
				Events = episode.Episodes_Events
								.Select(ep_ev => ep_ev.Event!)
								.ToList(),
				EventList = lists.Events!,
				CharacterList = lists.Characters!,
			};
		}
		private static Episode GetEpisodeFromModel(Episode episode, EpisodeViewModel model)
		{
			episode.Id = model.Id;
			episode.BookId = model.Book!.Id;
			episode.Heading = model.Heading;
			episode.Position = model.Position;
			episode.Content = model.Content;
			return episode;
		}

		
	}
}
