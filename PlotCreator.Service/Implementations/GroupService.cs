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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Service.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<IBaseResponse<GroupViewModel>> CreateGroup(GroupViewModel model)
        {
            var baseResponse = new BaseResponse<GroupViewModel>();
            try
            {
                var group = new Group()
                {
                    Name = model.Name,
                    Description = model.Description,
                    BookId = model.Book!.Id,
                    Parent = model.Parent,
                };
                await _groupRepository.Add(group);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<GroupViewModel>()
                {
                    Description = $"[GroupService | CreateCharacterGroup]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteGroup(int id)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var group = await _groupRepository.GetOne(id);
                await _groupRepository.Delete(group);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[GroupService | DeleteCharacterGroup]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<GroupViewModel>> EditGroup(GroupViewModel model)
        {
            var baseResponse = new BaseResponse<GroupViewModel>();
            try
            {
                var group = new Group()
                {
                    Id = model.Id,
                    BookId = model.Book!.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Parent = model.Parent,
                };
                await _groupRepository.Update(group);
                baseResponse.Data = model;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<GroupViewModel>()
                {
                    Description = $"[GroupService | EditCharacterGroup]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<GroupViewModel>>> GetAllGroupsByParent(int bookId, string parent)
        {
            var baseResponse = new BaseResponse<IEnumerable<GroupViewModel>>();
            try
            {
                var groups = await _groupRepository.GetAllGroupsByParent(bookId, parent);
                var data = new List<GroupViewModel>();
                foreach (var group in groups)
                {
                    var model = new GroupViewModel()
                    {
                        Id = group.Id,
                        Book = group.Book,
                        Name = group.Name,
                        Description = group.Description,
                        Parent = parent,
                    };
                    data.Add(model);
                }
                baseResponse.Data = data;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<GroupViewModel>>()
                {
                    Description = $"[GroupService | GetAllGroupsByParent]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Group>>> GetBookGroupsByParent(int bookId,string parent)
        {
            var baseResponse = new BaseResponse<IEnumerable<Group>>();
            try
            {
                var groups = await _groupRepository.GetAllGroupsByParent(bookId, parent);
                baseResponse.Data = groups;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Group>>()
                {
                    Description = $"[GroupService | GetBookIdeas]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }
    }
}
