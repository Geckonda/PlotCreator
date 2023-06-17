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
	public class EventService : IEventService
	{
		private readonly IEventRepository _eventRepository;
		public EventService(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
		}

		public async Task<bool> AddCharactersEventRelation(int eventId, int[] characterIds)
		{
			try
			{
				if (characterIds == null)
					return false;

				List<Event_Character> mediators = new();
				for (int i = 0; i < characterIds.Length; i++)
				{
					var mediator = new Event_Character()
					{
						EventId = eventId,
						CharacterId = characterIds[i],
					};
					mediators.Add(mediator);
				}
				await _eventRepository.AddCharactersToEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> AddEpisodesEventRelation(int eventId, int[] episodeIds)
		{
			try
			{
				if (episodeIds == null)
					return false;

				List<Episode_Event> mediators = new();
				for (int i = 0; i < episodeIds.Length; i++)
				{
					var mediator = new Episode_Event()
					{
						EventId = eventId,
						EpisodeId = episodeIds[i],
					};
					mediators.Add(mediator);
				}
				await _eventRepository.AddEpisodesToEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> AddGroupsEventRelation(int eventId, int[] groupIds)
		{
			try
			{
				if (groupIds == null)
					return false;

				List<Group_Event> mediators = new();
				for (int i = 0; i < groupIds.Length; i++)
				{
					var mediator = new Group_Event()
					{
						EventId = eventId,
						GroupId = groupIds[i],
					};
					mediators.Add(mediator);
				}
				await _eventRepository.AddGroupsToEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<IBaseResponse<EventViewModel>> CreateEvent(EventViewModel model)
		{
			var baseResponse = new BaseResponse<EventViewModel>();
			try
			{
				var Event = GetEventFromModel(new(), model);
				await _eventRepository.Add(Event);
				baseResponse.Data = model;
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<EventViewModel>()
				{
					Description = $"[EventService | CreateEvent]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
				};
			}
		}

		public async Task<bool> DeleteCharactersEventRelation(int eventId, int[] characterIds)
		{
			try
			{
				if (characterIds == null)
					return false;

				List<Event_Character> mediators = new();
				for (int i = 0; i < characterIds.Length; i++)
				{
					var mediator = new Event_Character()
					{
						EventId = eventId,
						CharacterId = characterIds[i],
					};
					mediators.Add(mediator);
				}
				await _eventRepository.DeleteCharactersFromEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> DeleteEpisodesEventRelation(int eventId, int[] episodeIds)
		{
			try
			{
				if (episodeIds == null)
					return false;

				List<Episode_Event> mediators = new();
				for (int i = 0; i < episodeIds.Length; i++)
				{
					var mediator = new Episode_Event()
					{
						EventId = eventId,
						EpisodeId = episodeIds[i],
					};
					mediators.Add(mediator);
				}
				await _eventRepository.DeleteEpisodesFromEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<IBaseResponse<bool>> DeleteEvent(int id)
		{
			var baseResponse = new BaseResponse<bool>();
			try
			{
				var Event =  _eventRepository.GetOne(id);
				if (Event == null)
				{
					baseResponse.Description = "Событие не найдено";
					baseResponse.StatusCode = StatusCode.NotFound;
					return baseResponse;
				}
				await _eventRepository.Delete(Event);
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
					Description = $"[EventService | DeleteEvent]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
				};
			}
		}

		public async Task<bool> DeleteGroupsEventRelation(int eventId, int[] groupIds)
		{
			try
			{
				List<Group_Event> mediators = new();
				for (int i = 0; i < groupIds.Length; i++)
				{
					var mediator = new Group_Event()
					{
						EventId = eventId,
						GroupId = groupIds[i],
					};
					mediators.Add(mediator);
				}
				await _eventRepository.DeleteGroupsFromEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> EditCharactersEventRelation(int eventId, int[] characterIds, int bookId)
		{
			try
			{
				List<Event_Character> mediators = new();
				for (int i = 0; i < characterIds.Length; i++)
				{
					var mediator = new Event_Character()
					{
						EventId = eventId,
						CharacterId = characterIds[i],
					};
					mediators.Add(mediator);
				}
				await _eventRepository.EditCharactersEntityRelation(mediators, eventId, bookId);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> EditEpisodesEventRelation(int eventId, int[] episodeIds, int bookId)
		{
			try
			{
				if (episodeIds == null)
					return false;

				List<Episode_Event> mediators = new();
				for (int i = 0; i < episodeIds.Length; i++)
				{
					var mediator = new Episode_Event()
					{
						EventId = eventId,
						EpisodeId = episodeIds[i],
					};
					mediators.Add(mediator);
				}
				await _eventRepository.EditEpisodesEntityRelation(mediators, eventId, bookId);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<IBaseResponse<EventViewModel>> EditEvent(EventViewModel model)
		{
			var baseResponse = new BaseResponse<EventViewModel>();
			try
			{
				var Event =  _eventRepository.GetOne(model.Id);
				if (Event == null)
				{
					baseResponse.Description = "Событие не найдено";
					baseResponse.StatusCode = StatusCode.NotFound;
					return baseResponse;
				}
				Event = GetEventFromModel(Event, model);
				await _eventRepository.Update(Event);
				model.Book = Event.Book;
				model.Book!.User = Event.Book!.User;

				baseResponse.Data = model;
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<EventViewModel>()
				{
					Description = $"[EventService | EditEvent]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
				};
			}
		}

		public async Task<bool> EditGroupsEventRelation(int eventId, int[] groupIds, int bookId)
		{
			try
			{
				List<Group_Event> mediators = new();
				for (int i = 0; i < groupIds.Length; i++)
				{
					var mediator = new Group_Event()
					{
						EventId = eventId,
						GroupId = groupIds[i],
					};
					mediators.Add(mediator);
				}
				await _eventRepository.EditGroupsEntityRelation(mediators, eventId, bookId);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public Task<IBaseResponse<IEnumerable<EventViewModel>>> GetAllEvents(int userId)
		{
			throw new NotImplementedException();
		}

		public async Task<Book> GetBook(int bookId)
		{
			try
			{
				return await _eventRepository.GetBook(bookId);
			}
			catch (Exception)
			{
				return null!;
			}
		}

		public async Task<IBaseResponse<IEnumerable<EventViewModel>>> GetBookEvents(int bookId)
		{
			var baseResponse = new BaseResponse<IEnumerable<EventViewModel>>();
			try
			{
				var Events = await _eventRepository.GetAllByBookId(bookId);
				var lists = await _eventRepository.GetReferenceData(bookId);
				List<EventViewModel> EventModels = new List<EventViewModel>();
				foreach (var Event in Events)
				{
					var model = GetModelFromEvent(Event, lists);
					EventModels.Add(model);
				}
				baseResponse.Data = EventModels;
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<IEnumerable<EventViewModel>>()
				{
					Description = $"[EventService | GetBookEvents]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
				};
			}
		}

		public async Task<int> GetBookId(int bookId)
		{
			try
			{
				var book = await _eventRepository.GetBook(bookId);
				return book.Id;
			}
			catch (Exception)
			{
				return (int)StatusCode.NotFound * -1;
			}
		}

		public async Task<IBaseResponse<EventViewModel>> GetEmptyViewModel(int bookId)
		{
			var baseResponse = new BaseResponse<EventViewModel>();
			try
			{
				var book = await _eventRepository.GetBook(bookId);
				var emptyModel = new EventViewModel()
				{
					Book = book,
				};
				baseResponse.Data = emptyModel;
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<EventViewModel>()
				{
					Description = $"[EventService | GetEmptyViewModel]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
				};
			}
		}

		public async Task<IBaseResponse<EventViewModel>> GetEvent(int eventId)
		{
			var baseResponse = new BaseResponse<EventViewModel>();
			try
			{
				var Event = _eventRepository.GetOne(eventId);
				var lists = await _eventRepository.GetReferenceData(Event.Book!.Id);
				if (Event == null)
				{
					baseResponse.Description = "Событие не найдено";
					baseResponse.StatusCode = StatusCode.NotFound;
					return baseResponse;
				}
				var model = GetModelFromEvent(Event, lists);
				baseResponse.Data = model;
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<EventViewModel>()
				{
					Description = $"[EventService | GetEvent]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
				};
			}
		}

		public async Task<int> GetLastUserEventId(int userId)
		{
			try
			{
				return await _eventRepository.GetLastUserEntityId(userId);
			}
			catch (Exception)
			{
				return (int)StatusCode.NotFound * -1;
			}
		}

		public async Task<User> GetUser(int eventId)
		{
			try
			{
				return await _eventRepository.GetUser(eventId);
			}
			catch (Exception)
			{
				return null!;
			}
		}

		public async Task<int> GetUserId(int eventId)
		{
			try
			{
				return await _eventRepository.GetUserId(eventId);
			}
			catch (Exception)
			{
				return (int)StatusCode.NotFound * -1;
			}
		}

		//
		private static EventViewModel GetModelFromEvent(Event Event, EventLists lists)
		{
			return new EventViewModel()
			{
				Id = Event.Id,
				Book = Event.Book,
				Title = Event.Title,
				Description = Event.Description,
				Beginning = Event.Beginning,
				Ending = Event.Ending,
				ChekhovsGun = Event.ChekhovsGun,
				IsHidden = Event.IsHidden,
				Icon = Event.Icon,
				Colour = Event.Colour,
				Episodes = Event.Episodes_Events
								.Select(ep_ev => ep_ev.Episode!)
								.ToList(),
				Characters = Event.Events_Characters
								.Select(ev_ch => ev_ch.Character!)
								.ToList(),
				Groups = Event.Groups_Events
								.Select(g_ev => g_ev.Group!)
								.ToList(),
				GroupList = lists!.Groups,
				EpisodeList = lists!.Episodes,
				CharacterList = lists.Characters!,
			};
		}

		private static Event GetEventFromModel(Event Event, EventViewModel model)
		{
			Event.Id = model.Id;
			Event.BookId = model.Book!.Id;
			Event.Title = model.Title;
			Event.Description = model.Description;
			Event.Beginning = model.Beginning;
			Event.Ending = model.Ending;
			Event.ChekhovsGun = model.ChekhovsGun;
			Event.IsHidden = model.IsHidden;
			Event.Icon = model.Icon;
			Event.Colour = model.Colour;
			return Event;
		}
	}
}
