using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.DAL.Repositories;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Multiple_Tables;
using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Helpers.Interfaces;
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
                await _ideaRepository.AddIdeasToBook(mediators);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[IdeaService | AddIdeasToBook]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
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
                    UserId = ideaViewModel.UserId,
                    Topic =  ideaViewModel.Topic,
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
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteIdea(int id)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var idea = await _ideaRepository.GetOne(id);
                if(idea == null)
                {
                    baseResponse.Description = "Идея не найдена";
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
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteIdeasFromBook(int bookId, int[] ideaIds)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var mediators = _ideaRepository.GetBookIdeasRelations(bookId, ideaIds);
                await _ideaRepository.DeleteIdeasFromBook(mediators);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[IdeaService | DeleteIdeasFromBook]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<Idea>> EditIdea(int id, IdeaViewModel ideaViewModel)
        {
            var baseResponse = new BaseResponse<Idea>();
            try
            {
                var idea = await _ideaRepository.GetOne(id);
                if (idea == null)
                {
                    baseResponse.Description = "Идеи не найдена";
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
                return new BaseResponse<Idea>()
                {
                    Description = $"[IdeaService | EditIdea]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
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
                    var model = new IdeaViewModel()
                    {
                        Id = idea.Id,
                        UserId = idea.UserId,
                        Topic = idea.Topic,
                        Data_Creation = idea.Data_Creation,
                        Content = idea.Content,
                    };
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
                };
            }
        }

        public async Task<IBaseResponse<IdeaViewModel>> GetIdea(int id)
        {
            var baseResponse = new BaseResponse<IdeaViewModel>();
            try
            {
                var idea = await _ideaRepository.GetOne(id);
                if (idea == null)
                {
                    baseResponse.Description = "Идея не найдена";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                var data = new IdeaViewModel()
                {
                    Id = id,
                    UserId = idea.UserId,
                    Data_Creation = idea.Data_Creation,
                    Topic = idea.Topic,
                    Content = idea.Content,
                };
                baseResponse.Data = data;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IdeaViewModel>()
                {
                    Description = $"[IdeaService | GetIdea]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
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
                    var model = new IdeaViewModel()
                    {
                        Id = idea.Id,
                        UserId = userId,
                        Topic = idea.Topic,
                        Data_Creation = idea.Data_Creation,
                        Content = idea.Content,
                    };
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
                    var model = new IdeaViewModel()
                    {
                        Id = idea.Id,
                        UserId = userId,
                        Topic = idea.Topic,
                        Data_Creation = idea.Data_Creation,
                        Content = idea.Content,
                    };
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
                };
            }
        }

        public async Task<int> GetLastUserIdeaId(int userId)
        {
            try
            {
                return await _ideaRepository.GetLastUserIdeaId(userId);
            }
            catch (Exception)
            {
                return -666;
            }
        }

        public async Task<int> GetUserId(int ideaId)
        {
            try
            {
                var idea = await _ideaRepository.GetAll()
                        .FirstOrDefaultAsync(x => x.Id == ideaId);
                return idea.UserId;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
