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
    public class IdeaService : IIdeaService
    {

        private readonly IIdeaRepository _ideaRepository;
        public IdeaService(IIdeaRepository ideaRepository)
        {
            _ideaRepository = ideaRepository;
        }
        public async Task<IBaseResponse<bool>> AddIdeasToBook(int bookId, int[] ideaIds)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                List<Book_Idea> mediators = new();
                for (int i = 0; i < ideaIds.Length; i++)
                {
                    var mediator = new Book_Idea()
                    {
                        BookId = bookId,
                        IdeaId = ideaIds[i],
                    };
                    mediators.Add(mediator);
                }
                await _ideaRepository.AddEntitiesToBook(mediators);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[IdeaService | AddIdeasToBook]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                    ErrorForUser = "Не удалось добавить идеи к книге.",
                };
            }
        }

        public async Task<IBaseResponse<IdeaViewModel>> CreateIdea(IdeaViewModel ideaViewModel)
        {
            var baseResponse = new BaseResponse<IdeaViewModel>();
            try
            {
                var idea = new Idea()
                {
                    UserId = ideaViewModel.User!.Id,
                    Topic = ideaViewModel.Topic,
                    Data_Creation = DateTime.Now,
                    Content = ideaViewModel.Content,
                };
                await _ideaRepository.Add(idea);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IdeaViewModel>()
                {
                    Description = $"[IdeaService | CreateIdea]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                    ErrorForUser = "Не удалось создать идею.",
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteIdea(int id)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var idea = _ideaRepository.GetOne(id);
                if (idea == null)
                {
                    baseResponse.ErrorForUser = "Идея не найдена";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                await _ideaRepository.Delete(idea);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[IdeaService | DeleteIdea]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                    ErrorForUser = "Не удалось удалить идею.",
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteIdeasFromBook(int bookId, int[] ideaIds)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var mediators = _ideaRepository.GetBookEntitiesRelations(bookId, ideaIds);
                await _ideaRepository.DeleteEntitiesFromBook(mediators);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[IdeaService | DeleteIdeasFromBook]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                    ErrorForUser = "Не удалось удалить идеи из книги.",
                };
            }
        }

        public async Task<IBaseResponse<IdeaViewModel>> EditIdea(int id, IdeaViewModel ideaViewModel)
        {
            var baseResponse = new BaseResponse<IdeaViewModel>();
            try
            {
                var idea =  _ideaRepository.GetOne(id);
                if (idea == null)
                {
                    baseResponse.ErrorForUser = "Идея не найдена";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                idea.Topic = ideaViewModel.Topic;
                idea.Content = ideaViewModel.Content;

                await _ideaRepository.Update(idea);
                baseResponse.StatusCode = StatusCode.Ok;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IdeaViewModel>()
                {
                    Description = $"[IdeaService | EditIdea]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                    ErrorForUser = "Не удалось отредактировать идею.",
                };
            }
        }

        public async Task<int> GetBookId(int bookId)
        {
            try
            {
                var book = await _ideaRepository.GetBook(bookId);
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
                return await _ideaRepository.GetBook(bookId);
            }
            catch (Exception)
            {
                return null!;
            }
        }

        public async Task<IBaseResponse<IEnumerable<IdeaViewModel>>> GetBookIdeas(int bookId)
        {
            var baseResponse = new BaseResponse<IEnumerable<IdeaViewModel>>();
            try
            {
                var ideas = await _ideaRepository.GetAllByBookId(bookId);
                List<IdeaViewModel> ideaModels = new();
                foreach (var idea in ideas)
                {
                    var model = GetModelFromIdea(idea);
                    ideaModels.Add(model);
                }
                baseResponse.Data = ideaModels;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<IdeaViewModel>>()
                {
                    Description = $"[IdeaService | GetBookIdeas]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                    ErrorForUser = "Не удалось получить идеи книги.",
                };
            }
        }

        public async Task<IBaseResponse<IdeaViewModel>> GetIdea(int id)
        {
            var baseResponse = new BaseResponse<IdeaViewModel>();
            try
            {
                var idea =  _ideaRepository.GetOne(id);
                if (idea == null)
                {
                    baseResponse.ErrorForUser = "Идея не найдена";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                var model = GetModelFromIdea(idea);
                baseResponse.Data = model;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IdeaViewModel>()
                {
                    Description = $"[IdeaService | GetIdea]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                    ErrorForUser = "Не удалось получить идею.",
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<IdeaViewModel>>> GetIdeaExcludeBook(int userId, int bookId)
        {
            var baseResponse = new BaseResponse<IEnumerable<IdeaViewModel>>();
            try
            {
                var ideas = await _ideaRepository.GetAllExcludeCurrentBookIdeas(userId, bookId);
                List<IdeaViewModel> ideaModels = new List<IdeaViewModel>();
                foreach (var idea in ideas)
                {
                    var model = GetModelFromIdea(idea);
                    ideaModels.Add(model);
                }
                baseResponse.Data = ideaModels;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<IdeaViewModel>>()
                {
                    Description = $"[IdeaService | GetIdeaExcludeBook]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                    ErrorForUser = "Не удалось получить идеи.",
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<IdeaViewModel>>> GetIdeas(int userId)
        {
            var baseResponse = new BaseResponse<IEnumerable<IdeaViewModel>>();
            try
            {
                var ideas = await _ideaRepository.GetAllByUserId(userId);
                List<IdeaViewModel> ideaModels = new();
                foreach (var idea in ideas)
                {
                    var model = GetModelFromIdea(idea);
                    ideaModels.Add(model);
                }
                baseResponse.Data = ideaModels;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<IdeaViewModel>>()
                {
                    Description = $"[IdeaService | GetIdeas]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                    ErrorForUser = "Не удалось получить идеи.",
                };
            }
        }

        public async Task<IBaseResponse<IdeaViewModel>> GetLastUserIdea(int userId)
        {
            var baseResponse = new BaseResponse<IdeaViewModel>();
            try
            {
                var id = await _ideaRepository.GetLastUserEntityId(userId);
                var idea = _ideaRepository.GetOne(id);
                var model = GetModelFromIdea(idea);
                baseResponse.Data = model;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IdeaViewModel>()
                {
                    Description = $"[IdeaService | GetLastUserIdea]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                    ErrorForUser = "Последняя идея не найдена",
                };
            }
        }
        public async Task<User> GetUser(int ideaId)
        {
            try
            {
                return await _ideaRepository.GetUser(ideaId);
            }
            catch (Exception)
            {
                return null!;
            }
        }

        public async Task<int> GetUserId(int ideaId)
        {
            try
            {
                return await _ideaRepository.GetUserId(ideaId);
            }
            catch (Exception)
            {
                return (int)StatusCode.NotFound * -1;
            }
        }


        private static IdeaViewModel GetModelFromIdea(Idea idea)
        {
            return new IdeaViewModel()
            {
                Id = idea.Id,
                User = idea.User,
                Topic = idea.Topic,
                Data_Creation = idea.Data_Creation,
                Content = idea.Content,
                Books = idea.Books_Ideas
                            .Select(book => book.Book!)
                            .ToList(),
            };
        }

        
    }
}
