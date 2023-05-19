using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.DAL.Repositories;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Multiple_Tables;
using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Response.Implementations;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
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
            _characterRepository= characterRepository;
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
				};
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
                var character = new Character()
                {
                    Id = model.Id,
                    UserId = model.UserId,
                    Name = model.Name,
                    Birthday = model.Birthday,
                    Gender = model.Gender,
                    Height = model.Height,
                    Weight = model.Weight,
                    Personality = model.Personality,
                    Appearance = model.Appearance,
                    Goals = model.Goals,
                    Motivation = model.Motivation,
                    History = model.History,
                    WorldviewId = model.WorldviewId,
                    Picture = model.Picture,
                    Deathday = model.Deathday,
                };
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
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteCharacter(int id)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var character = await _characterRepository.GetOne(id);
                if(character == null)
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
                };
            }

        }

        public async Task<IBaseResponse<bool>> DeleteCharactersFromBook(int bookId, int[] characterIds)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
				var mediators =  _characterRepository.GetBookEntitiesRelations(bookId, characterIds);
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
                };
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
                var character = await _characterRepository.GetOne(model.Id);
                if(character == null)
                {
                    baseResponse.Description = "Персонаж ненайден";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
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
                character.WorldviewId = model.WorldviewId;
                character.Picture = model.Picture;
                character.Deathday = model.Deathday;

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
                };
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
                await _characterRepository.EditGroupsEntityRelation(mediators,characterId, bookId);
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
                List<CharacterViewModel> characterModels = new List<CharacterViewModel>();
                foreach (var character in characters)
                {
                    var model = new CharacterViewModel()
                    {
                        Id = character.Id,
                        UserId = character.UserId,
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
                        WorldviewId = character.WorldviewId,
                        Worldview = character.Worldview,
                        Worldviews = character.Worldviews,
                        Picture = character.Picture,
                        Deathday = character.Deathday,
                    };
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
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<CharacterViewModel>>> GetBookCharacters(int bookId)
        {
            var baseResponse = new BaseResponse<IEnumerable<CharacterViewModel>>();
            try
            {
                var characters = await _characterRepository.GetAllByBookId(bookId);
                List<CharacterViewModel> characterModels = new List<CharacterViewModel>();
                foreach (var character in characters)
                {
                    var model = new CharacterViewModel()
                    {
                        Id = character.Id,
                        UserId = character.UserId,
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
                        WorldviewId = character.WorldviewId,
                        Worldview = character.Worldview,
                        Worldviews = character.Worldviews,
                        Picture = character.Picture,
                        Deathday = character.Deathday,
                    };
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
                };
            }
        }

        public async Task<IBaseResponse<CharacterViewModel>> GetCharacter(int id)
        {
            var baseResponse = new BaseResponse<CharacterViewModel>();
            try
            {
                var character = await _characterRepository.GetOne(id);
                if(character == null)
                {
                    baseResponse.Description = "Персонаж не найден";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                var data = new CharacterViewModel()
                {
                    Id = character.Id,
                    UserId = character.UserId,
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
                    WorldviewId = character.WorldviewId,
                    Worldview = character.Worldview,
                    Worldviews = character.Worldviews,
                    Picture = character.Picture,
                    Deathday = character.Deathday,
                    Groups = character.Groups,
                    OwnGroups = character.Groups_Characters
                };
                baseResponse.Data = data;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<CharacterViewModel>()
                {
                    Description = $"[CharacterService | GetCharacter]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

		public async Task<IBaseResponse<IEnumerable<CharacterViewModel>>> GetCharacterExcludeBook(int userId, int bookId)
		{
			var baseResponse = new BaseResponse<IEnumerable<CharacterViewModel>>();
			try
			{
				var characters = await _characterRepository.GetAllExcludeCurrentBookCharacters(userId, bookId);
				List<CharacterViewModel> characterModels = new List<CharacterViewModel>();
				foreach (var character in characters)
				{
					var model = new CharacterViewModel()
					{
						Id = character.Id,
						UserId = character.UserId,
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
						WorldviewId = character.WorldviewId,
						Worldview = character.Worldview,
						Worldviews = character.Worldviews,
						Picture = character.Picture,
						Deathday = character.Deathday,
						Groups = character.Groups,
					};
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
				};
			}
		}

		public async Task<IBaseResponse<CharacterViewModel>> GetEmptyViewModel(int bookId)
		{
            var baseResponse = new BaseResponse<CharacterViewModel>();
            try
            {
                var emptyCharactrer = await _characterRepository.GetEmptyViewModel(bookId);
				var emptyModel = new CharacterViewModel()
				{
                    Worldviews = emptyCharactrer.Worldviews,
                    Groups = emptyCharactrer.Groups,
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
                return  await _characterRepository.GetLastUserCharacterId(userId);
            }
            catch (Exception)
            {
                return -666;
            }
        }

        public async Task<int> GetUserId(int characterId)
        {
            try
            {
				var character = await _characterRepository.GetOne(characterId);
				return character.UserId;
			}
			catch (Exception)
			{
				return -666;
			}
		}
    }
}
