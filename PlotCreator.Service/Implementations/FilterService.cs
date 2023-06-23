using PlotCreator.DAL.Interfaces;
using PlotCreator.DAL.Repositories;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Filters;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlotCreator.Service.Implementations
{
	public class FilterService : IFilterService
	{
		private readonly ICharacterRepository _characterRepository;
		private readonly IEventRepository _eventRepository;
		public FilterService(ICharacterRepository characterRepository, IEventRepository eventRepository)
		{
			_characterRepository = characterRepository;
			_eventRepository = eventRepository;
		}
		public async Task<IBaseResponse<IEnumerable<CharacterViewModel>>> FilterAllUserCharacters(CharacterFilter characterFilter)
		{
			var baseResponse = new BaseResponse<IEnumerable<CharacterViewModel>>();
			try
			{
				IQueryable<Character> characters;
				if(characterFilter.BookId > 0)
				{
					characters = await _characterRepository.GetAllByBookId(characterFilter.BookId);
				}
				else
				{
					characters = await _characterRepository.GetAllByUserId(characterFilter.UserId);
				}
				var lists = await _characterRepository.GetReferenceData();
				characters = FilterCharacters(characters, characterFilter);
				List<CharacterViewModel> characterModels = new List<CharacterViewModel>();
				foreach (var character in characters)
				{
					var model = GetModelFromCharacter(character, lists);
					characterModels.Add(model);
				}
				baseResponse.Data = characterModels;
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<IEnumerable<CharacterViewModel>>()
				{
					Description = $"[FilterService | FilterAllUserCharacters]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
					ErrorForUser = "Не удалось получить отфильтрованных персонажей",
				};
			}
		}
		public async Task<IBaseResponse<IEnumerable<EventViewModel>>> FilterAllUserEvents(EventFilter eventFilter)
		{
			var baseResponse = new BaseResponse<IEnumerable<EventViewModel>>();
			try
			{
				var Events = await _eventRepository.GetAllByBookId(eventFilter.BookId);
				var lists = await _eventRepository.GetReferenceData(eventFilter.BookId);
				Events = FilterEvents(Events, eventFilter);
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
					Description = $"[FilterService | FilterAllUserEvents]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
				};
			}
		}
		private IQueryable<Event> FilterEvents(IQueryable<Event> events, EventFilter eventFilter)
		{
			if((eventFilter!.Date!.From.ToString("d") != "1/1/0001") &&
                (eventFilter!.Date!.From.ToString("d") != "01.01.0001"))
			{
				events = events.Where(x => x.Beginning >= eventFilter!.Date!.From);
			}
			if ((eventFilter!.Date!.To.ToString("d") != "1/1/0001") &&
                (eventFilter!.Date!.To.ToString("d") != "01.01.0001"))
			{
				events = events.Where(x => x.Ending <= eventFilter!.Date!.To);
			}
			if (eventFilter.CheckhovsGun)
			{
				events = events.Where(x => x.ChekhovsGun == true);
			}
			if (eventFilter.IsHidden)
			{
				events = events.Where(x => x.IsHidden == true);
			}
			if (eventFilter!.Groups!.CheckedObjects.Length > 0)
			{
				var groups = events.Select(x => x.Groups_Events.Select(x => x.GroupId));
				List<Event> eventList = new List<Event>();
				var gr = groups.ToArray();
				var ev = events.ToArray();
				for (int i = 0; i < groups.Count(); i++)
				{
					if (gr[i].Any(x => eventFilter!.Groups!.CheckedObjects.Contains(x)))
						eventList.Add(ev[i]);
				}
				events = eventList.AsQueryable();
			}
			return events;
		}
		private IQueryable<Character> FilterCharacters(IQueryable<Character> characters, CharacterFilter characterFilter)
		{
			var x = characterFilter!.Date!.From.ToString("d");
            if ((characterFilter!.Date!.From.ToString("d") != "1/1/0001") &&
                (characterFilter!.Date!.From.ToString("d") != "01.01.0001"))
			{
				characters = characters.Where(x => x.Birthday >= characterFilter!.Date!.From);
			}
			if ((characterFilter!.Date!.To.ToString("d") != "1/1/0001") &&
                (characterFilter!.Date!.To.ToString("d") != "01.01.0001"))
			{
				characters = characters.Where(x => x.Birthday <= characterFilter!.Date.To);
			}
			if (characterFilter.Gender.Length > 0)
			{
				characters = characters.Where(x => x.Gender!
										.Contains(characterFilter.Gender[0]));
			}
			if(characterFilter!.Height!.From != 0)
			{
				characters = characters.Where(x => x.Height >= characterFilter!.Height!.From);
			}
			if (characterFilter!.Height!.To != 0)
			{
				characters = characters.Where(x => x.Height <= characterFilter!.Height.To);
			}
			if (characterFilter!.Weight!.From != 0)
			{
				characters = characters.Where(x => x.Weight >= characterFilter!.Weight!.From);
			}
			if (characterFilter!.Weight!.To != 0)
			{
				characters = characters.Where(x => x.Weight <= characterFilter!.Weight.To);
			}
			if (characterFilter!.Worldviews!.CheckedObjects.Length > 0)
			{
				characters = characters.Where(x => characterFilter.Worldviews!.CheckedObjects
										.Contains(x.WorldviewId));
			}
			if (characterFilter!.Groups!.CheckedObjects.Length > 0)
			{
				var groups = characters.Select(x => x.Groups_Characters.Select(x => x.GroupId));
				List<Character> characterList = new List<Character>();
				var gr = groups.ToArray();
				var ch = characters.ToArray();
				for (int i = 0; i < groups.Count(); i++)
				{
					if (gr[i].Any(x => characterFilter!.Groups!.CheckedObjects.Contains(x)))
						characterList.Add(ch[i]);
				}
				characters = characterList.AsQueryable();
			}
			return characters;
		}
		private static CharacterViewModel GetModelFromCharacter(Character character, CharacterLists? lists)
		{
			return new CharacterViewModel()
			{
				Id = character.Id,
				User = character.User,
				Name = character.Name,
				Birthday = character.Birthday,
				Gender = character.Gender,
				Height = character.Height,
				Weight = character.Weight,
				Personality = character.Personality,
				Appearance = character.Appearance,
				Goals = character.Goals,
				Motivation = character.Motivation,
				History = character.History,
				Conflict = character.Conflict,
				Worldview = character.Worldview,
				Picture = character.Picture,
				Deathday = character.Deathday,
				Books = character.Books_Characters
									.Select(b_ch => b_ch.Book!)
									.ToList(),
				Groups = character.Groups_Characters
									.Select(g_ch => g_ch.Group!)
									.ToList(),
				Events = character.Events_Characters
									.Select(ev_ch => ev_ch.Event!)
									.ToList(),
				Episodes = character.Episodes_Characters
									.Select(ep_ch => ep_ch.Episode!)
									.ToList(),
				Worldviews = lists!.Worldviews,
				GroupList = lists!.Groups,
				EpisodeList = lists!.Episodes,
				EventList = lists!.Events,
			};
		}
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

	}
}
