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
				var group = GetGroupFromModel(new(),model);
				await _groupRepository.Add(group);
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<GroupViewModel>()
				{
					Description = $"[GroupService | CreateGroup]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
				};
			}
		}

		public async Task<IBaseResponse<bool>> DeleteGroup(int id)
		{
			var baseResponse = new BaseResponse<bool>();
			try
			{
				var group = _groupRepository.GetOne(id);
				await _groupRepository.Delete(group);
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
					Description = $"[GroupService | DeleteGroup]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
				};
			}
		}

		public async Task<IBaseResponse<GroupViewModel>> EditGroup(GroupViewModel model)
		{
			var baseResponse = new BaseResponse<GroupViewModel>();
			try
			{
				var group = GetGroupFromModel(new(), model);
				await _groupRepository.Update(group);
				baseResponse.Data = model;
				baseResponse.StatusCode = StatusCode.Ok;
				return baseResponse;
			}
			catch (Exception ex)
			{
				return new BaseResponse<GroupViewModel>()
				{
					Description = $"[GroupService | EditGroup]: {ex.Message}",
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
					var model = GetModelFromGroup(group);
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

		public async Task<IBaseResponse<IEnumerable<Group>>> GetBookGroupsByParent(int bookId, string parent)
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
					Description = $"[GroupService | GetBookGroupsByParent]: {ex.Message}",
					StatusCode = StatusCode.InternalServerError,
				};
			}
		}
		//
		private GroupViewModel GetModelFromGroup(Group group)
		{
			return new GroupViewModel()
			{
				Id = group.Id,
				Book = group.Book,
				Name = group.Name,
				Description = group.Description,
				Parent = group.Parent,
				Characters = group.Groups_Characters.
								Select(x => x.Character!).ToList(),
				Events = group.Groups_Events
							.Select(x => x.Event!).ToList(),

			};
		}
		private Group GetGroupFromModel(Group group, GroupViewModel model)
		{
			group.Id = model.Id;
			group.Name = model.Name;
			group.Description = model.Description;
			group.BookId = model.Book!.Id;
			group.Parent = model.Parent;
			return group;
		}
	}
}
