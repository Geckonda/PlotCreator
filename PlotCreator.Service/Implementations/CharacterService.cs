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
	public class CharacterService : ICharacterService
	{
		private readonly ICharacterRepository _characterRepository;
		public CharacterService(ICharacterRepository characterRepository)
		{
			_characterRepository = characterRepository;
		}
		public async Task<IBaseResponse<bool>> AddCharactersToBook(int bookId, int[] characterIds)
		{
			var baseResponse = new BaseResponse<bool>();
			try
			{
				List<Book_Character> mediators = new();
				for (int i = 0; i < characterIds.Length; i++)
				{
					var mediator = new Book_Character()
					{
						BookId = bookId,
						CharacterId = characterIds[i],
					};
					mediators.Add(mediator);
				}
				await _characterRepository.AddEntitiesToBook(mediators);
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
					Description = $"[CharacterService | AddCharactersToBook]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
					ErrorForUser = "Не удалоась добавить персонажа к книге."
				};
			}
		}

		public async Task<bool> AddEpisodesCharacterRelation(int characterId, int[] episodeIds)
		{
			try
			{
				if (episodeIds == null)
					return false;

				List<Episode_Character> mediators = new();
				for (int i = 0; i < episodeIds.Length; i++)
				{
					var mediator = new Episode_Character()
					{
						CharacterId = characterId,
						EpisodeId = episodeIds[i],
					};
					mediators.Add(mediator);
				}
				await _characterRepository.AddEpisodesToEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> AddEventsCharacterRelation(int characterId, int[] eventIds)
		{
			try
			{
				if (eventIds == null)
					return false;

				List<Event_Character> mediators = new();
				for (int i = 0; i < eventIds.Length; i++)
				{
					var mediator = new Event_Character()
					{
						CharacterId = characterId,
						EventId = eventIds[i],
					};
					mediators.Add(mediator);
				}
				await _characterRepository.AddEventsToEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> AddGroupsCharacterRelation(int characterId, int[] groupIds)
		{
			try
			{
				if (groupIds == null)
					return false;

				List<Group_Character> mediators = new();
				for (int i = 0; i < groupIds.Length; i++)
				{
					var mediator = new Group_Character()
					{
						CharacterId = characterId,
						GroupId = groupIds[i],
					};
					mediators.Add(mediator);
				}
				await _characterRepository.AddGroupsToEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<IBaseResponse<CharacterViewModel>> CreateCharacter(CharacterViewModel model)
		{
			var baseResponse = new BaseResponse<CharacterViewModel>();
			try
			{
				var character = GetCharacterFromModel(new Character(), model);
				await _characterRepository.Add(character);
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<CharacterViewModel>()
				{
					Description = $"[CharacterService | CreateCharacter]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
					ErrorForUser = "Не удалось добавить персонажа",
				};
			}
		}

		public async Task<IBaseResponse<bool>> DeleteCharacter(int id)
		{
			var baseResponse = new BaseResponse<bool>();
			try
			{
				var character =  _characterRepository.GetOne(id);
				if (character == null)
				{
					baseResponse.Description = "Персонаж не найден";
					baseResponse.StatusCode = StatusCode.NotFound;
					return baseResponse;
				}
				await _characterRepository.Delete(character);
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
					Description = $"[CharacterService | DeleteCharacter]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
					ErrorForUser = "Не удалось удалить персонажа",
				};
			}
		}

		public async Task<IBaseResponse<bool>> DeleteCharactersFromBook(int bookId, int[] characterIds)
		{
			var baseResponse = new BaseResponse<bool>();
			try
			{
				var mediators = _characterRepository.GetBookEntitiesRelations(bookId, characterIds);
				await _characterRepository.DeleteEntitiesFromBook(mediators);
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
					Description = $"[CharacterService | DeleteCharactersFromBook]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
					ErrorForUser = "Не удалось удалить персонажа из книги",
				};
			}
		}

		public async Task<bool> DeleteEpisodesCharacterRelation(int characterId, int[] episodeIds)
		{
			try
			{
				List<Episode_Character> mediators = new();
				for (int i = 0; i < episodeIds.Length; i++)
				{
					var mediator = new Episode_Character()
					{
						CharacterId = characterId,
						EpisodeId = episodeIds[i],
					};
					mediators.Add(mediator);
				}
				await _characterRepository.DeleteEpisodesFromEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> DeleteEventsCharacterRelation(int characterId, int[] eventIds)
		{
			try
			{
				List<Event_Character> mediators = new();
				for (int i = 0; i < eventIds.Length; i++)
				{
					var mediator = new Event_Character()
					{
						CharacterId = characterId,
						EventId = eventIds[i],
					};
					mediators.Add(mediator);
				}
				await _characterRepository.DeleteEventsFromEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> DeleteGroupsCharacterRelation(int characterId, int[] groupIds)
		{
			try
			{
				List<Group_Character> mediators = new();
				for (int i = 0; i < groupIds.Length; i++)
				{
					var mediator = new Group_Character()
					{
						CharacterId = characterId,
						GroupId = groupIds[i],
					};
					mediators.Add(mediator);
				}
				await _characterRepository.DeleteGroupsFromEntity(mediators);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<IBaseResponse<CharacterViewModel>> EditCharacter(CharacterViewModel model)
		{
			var baseResponse = new BaseResponse<CharacterViewModel>();
			try
			{
				var character =  _characterRepository.GetOne(model.Id);
				if (character == null)
				{
					baseResponse.Description = "Персонаж ненайден";
					baseResponse.StatusCode = StatusCode.NotFound;
					return baseResponse;
				}
				character = GetCharacterFromModel(character,model);

				await _characterRepository.Update(character);
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<CharacterViewModel>()
				{
					Description = $"[CharacterService | EditCharacter]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
					ErrorForUser = "Не удалось отредактировать персонажа",
				};
			}
		}

		public async Task<bool> EditEpisodesCharacterRelation(int characterId, int[] episodeIds, int bookId)
		{
			try
			{
				List<Episode_Character> mediators = new();
				for (int i = 0; i < episodeIds.Length; i++)
				{
					var mediator = new Episode_Character()
					{
						CharacterId = characterId,
						EpisodeId = episodeIds[i],
					};
					mediators.Add(mediator);
				}
				await _characterRepository.EditEpisodesEntityRelation(mediators, characterId, bookId);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> EditEventsCharacterRelation(int characterId, int[] eventIds, int bookId)
		{
			try
			{
				List<Event_Character> mediators = new();
				for (int i = 0; i < eventIds.Length; i++)
				{
					var mediator = new Event_Character()
					{
						CharacterId = characterId,
						EventId = eventIds[i],
					};
					mediators.Add(mediator);
				}
				await _characterRepository.EditEventsEntityRelation(mediators, characterId, bookId);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> EditGroupsCharacterRelation(int characterId, int[] groupIds, int bookId)
		{
			try
			{
				List<Group_Character> mediators = new();
				for (int i = 0; i < groupIds.Length; i++)
				{
					var mediator = new Group_Character()
					{
						CharacterId = characterId,
						GroupId = groupIds[i],
					};
					mediators.Add(mediator);
				}
				await _characterRepository.EditGroupsEntityRelation(mediators, characterId, bookId);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<IBaseResponse<IEnumerable<CharacterViewModel>>> GetAllCharacters(int userId)
		{
			var baseResponse = new BaseResponse<IEnumerable<CharacterViewModel>>();
			try
			{
				var characters = await _characterRepository.GetAllByUserId(userId);
				var lists = await _characterRepository.GetReferenceData();
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
					Description = $"[CharacterService | GetAllCharacters]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
					ErrorForUser = "Не удалось получить всех персонажей",
				};
			}
		}

		public async Task<IBaseResponse<IEnumerable<CharacterViewModel>>> GetBookCharacters(int bookId)
		{
			var baseResponse = new BaseResponse<IEnumerable<CharacterViewModel>>();
			try
			{
				var characters = await _characterRepository.GetAllByBookId(bookId);
				var lists = await _characterRepository.GetReferenceData(bookId);
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
					Description = $"[CharacterService | GetBookCharacters]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
					ErrorForUser = "Не удалось получить всех персонажей книги",
				};
			}
		}

		public async Task<IBaseResponse<CharacterViewModel>> GetCharacter(int id)
		{
			var baseResponse = new BaseResponse<CharacterViewModel>();
			try
			{
				var character =  _characterRepository.GetOne(id);
				var lists = await _characterRepository.GetReferenceData();
				if (character == null)
				{
					baseResponse.Description = "Персонаж не найден";
					baseResponse.StatusCode = StatusCode.NotFound;
					return baseResponse;
				}
				var model = GetModelFromCharacter(character, lists);
				baseResponse.Data = model;
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<CharacterViewModel>()
				{
					Description = $"[CharacterService | GetCharacter]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
					ErrorForUser = "Не удалось получить персонажа",
				};
			}
		}

		public async Task<IBaseResponse<IEnumerable<CharacterViewModel>>> GetCharactersExcludeBook(int userId, int bookId)
		{
			var baseResponse = new BaseResponse<IEnumerable<CharacterViewModel>>();
			try
			{
				var characters = await _characterRepository.GetAllExcludeCurrentBookCharacters(userId, bookId);
				var lists = await _characterRepository.GetReferenceData(bookId);
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
					Description = $"[CharacterService | GetBookCharacters]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
					ErrorForUser = "Не удалось получить персонажей",
				};
			}
		}

		public async Task<IBaseResponse<CharacterViewModel>> GetEmptyViewModel(int bookId)
		{
			var baseResponse = new BaseResponse<CharacterViewModel>();
			try
			{
				var lists = await _characterRepository.GetReferenceData(bookId);
				var emptyModel = new CharacterViewModel()
				{
					Worldviews = lists.Worldviews,
					GroupList = lists.Groups,
					EpisodeList = lists.Episodes,
					EventList = lists.Events,
				};
				baseResponse.Data = emptyModel;
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<CharacterViewModel>()
				{
					Description = $"[CharacterService | GetEmptyViewModel]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
				};
			}
		}

		public async Task<int> GetLastUserCharacterId(int userId)
		{
			try
			{
				return await _characterRepository.GetLastUserEntityId(userId);
			}
			catch (Exception)
			{
				return (int)StatusCode.NotFound * -1;
			}
		}

		public async Task<int> GetUserId(int characterid)
		{
			try
			{
				return await _characterRepository.GetUserId(characterid);
			}
			catch (Exception)
			{
				return (int)StatusCode.NotFound * -1;
			}
		}

		public async Task<User> GetUser(int characterid)
		{
			try
			{
				return await _characterRepository.GetUser(characterid);
			}
			catch (Exception)
			{
				return null!;
			}
		}

		public async Task<int> GetBookId(int bookId)
		{
			try
			{
				var book = await _characterRepository.GetBook(bookId);
				return book.Id;
			}
			catch (Exception)
			{
				return (int)StatusCode.NotFound * -1;
			}
		}

		public async Task<Book> GetBook(int bookId)
		{
			try
			{
				return await _characterRepository.GetBook(bookId);
			}
			catch (Exception)
			{
				return null!;
			}
		}


		/// <summary>
		/// Конвертирует объект <see cref="Character" /> в объект <see cref="CharacterViewModel"/>
		/// </summary>
		/// <param name="book"></param>
		/// <param name="lists">Список справочников</param>
		/// <returns></returns>
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
		private static Character GetCharacterFromModel(Character character,CharacterViewModel model)
		{
			character.UserId = model.User!.Id;
			character.Name = model.Name;
			character.Birthday = model.Birthday;
			character.Gender = model.Gender;
			character.Height = model.Height;
			character.Weight = model.Weight;
			character.Personality = model.Personality;
			character.Appearance = model.Appearance;
			character.Goals = model.Goals;
			character.Motivation = model.Motivation;
			character.History = model.History;
			character.WorldviewId = model.Worldview!.Id;
			character.Picture = model.Picture;
			character.Conflict = model.Conflict;
			character.Deathday = model.Deathday;
			return character;
		}

		
	}
}
