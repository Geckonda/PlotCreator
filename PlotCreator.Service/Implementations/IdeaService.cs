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
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Service.Implementations
{
    public class IdeaService : IIdeaService
    {
        private readonly IBaseRepository<Idea> _ideaRepository;
        public IdeaService(IBaseRepository<Idea> ideaRepository)
        {
            _ideaRepository = ideaRepository;
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
                var idea = await _ideaRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task<IBaseResponse<Idea>> EditIdea(int? id, IdeaViewModel ideaViewModel)
        {
            var baseResponse = new BaseResponse<Idea>();
            try
            {
                var idea = await _ideaRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task<IBaseResponse<IdeaViewModel>> GetIdea(int id)
        {
            var baseResponse = new BaseResponse<IdeaViewModel>();
            try
            {
                var idea = await _ideaRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task<IBaseResponse<IEnumerable<Idea>>> GetIdeas(int userId)
        {
            var baseResponse = new BaseResponse<IEnumerable<Idea>>();
            try
            {
                var ideas = _ideaRepository.GetAll()
                    .Where(x => x.UserId == userId);
                if (!ideas.Any())
                {
                    baseResponse.Description = "Идея не найдена";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                baseResponse.Data = ideas;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Idea>>()
                {
                    Description = $"[IdeaService | GetIdea]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<int> GetUserId(int ideaId)
        {
            var idea = await _ideaRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == ideaId);
            return idea.UserId;
        }
    }
}
