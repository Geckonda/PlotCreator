using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.DAL.Repositories;
using PlotCreator.Domain.Entity;
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
        private readonly ICharacterRepository<Character> _characterRepository;
        public CharacterService(ICharacterRepository<Character> characterRepository)
        {
            _characterRepository= characterRepository;
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

        public async Task<IBaseResponse<IEnumerable<CharacterViewModel>>> GetAllCharacters(int userId)
        {
            var baseResponse = new BaseResponse<IEnumerable<CharacterViewModel>>();
            try
            {
                var characters = await _characterRepository.GetAllCharacters(userId);
                /* if (!characters.Any())
                 {
                     baseResponse.Description = "Не найдено ни одного персонажа";
                     baseResponse.StatusCode = StatusCode.NotFound;
                 }*/
                List<CharacterViewModel> charactersModel = new List<CharacterViewModel>();
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
                    charactersModel.Add(model);
                }
                baseResponse.Data = charactersModel;
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
                var characters = await _characterRepository.GetBookCharacters(bookId);
               /* if (!characters.Any())
                {
                    baseResponse.Description = "Не найдено ни одного персонажа";
                    baseResponse.StatusCode = StatusCode.NotFound;
                }*/
                List<CharacterViewModel> charactersModel = new List<CharacterViewModel>();
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
                    charactersModel.Add(model);
                }
                baseResponse.Data = charactersModel;
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

		public async Task<IBaseResponse<CharacterViewModel>> GetEmptyViewModel()
		{
            var baseResponse = new BaseResponse<CharacterViewModel>();
            try
            {
                var emptyModel = await _characterRepository.GetEmptyViewModel();
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

		public async Task<int> GetUserId(int characterId)
        {
            try
            {
				var character = await _characterRepository.GetOne(characterId);
				return character.UserId;
			}
			catch (Exception)
			{
				return 0;
			}
		}
    }
}
