using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Implementations;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
		private readonly IGroupService _groupService;
		private readonly IBookService _bookService;
		public GroupsController(IGroupService groupService, IBookService bookService)
		{
			_groupService = groupService;
			_bookService = bookService;
		}
		[HttpGet]
		public async Task<IActionResult> GetCharactersGroups(int bookId)
		{
            if (!UserIsBookOwner(bookId).Result)
                return View("404");

            var response = await _groupService.GetAllGroupsByParent(bookId, "Character");
			ViewData["bookId"] = bookId;
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);
			return RedirectToAction("Error");
		}
		[HttpGet]
		public async Task<IActionResult> GetEventsGroups(int bookId)
		{
            if (!UserIsBookOwner(bookId).Result)
                return View("404");

            var response = await _groupService.GetAllGroupsByParent(bookId, "Event");
			ViewData["bookId"] = bookId;
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);
			return RedirectToAction("Error");
		}

		[HttpPost]
		public async Task<IActionResult> Save(GroupViewModel newGroup)
		{
			var response = await _groupService.CreateGroup(newGroup);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
			{
				if (newGroup.Parent == "Event")
					return RedirectToRoute(new { controller = "Groups", action = "GetEventsGroups", bookId = newGroup.Book!.Id });

				return RedirectToRoute(new { controller = "Groups", action = "GetCharactersGroups", bookId = newGroup.Book!.Id });
			}
			return RedirectToAction("Error");
		}
		[HttpGet]
		public async Task<IActionResult> Delete(int id, int bookId, string parent)
		{
            if (!UserIsBookOwner(bookId).Result)
                return View("404");

            var response = await _groupService.DeleteGroup(id);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
			{
				if (parent == "Event")
					return RedirectToRoute(new { controller = "Groups", action = "GetEventsGroups", bookId = bookId });
				return RedirectToRoute(new { controller = "Groups", action = "GetCharactersGroups", bookId = bookId });
			}
			return RedirectToAction("Error");
		}

		[HttpPost]
		public async Task<IActionResult> Edit(GroupViewModel currentGroup, int bookId)
		{
			var response = await _groupService.EditGroup(currentGroup);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
			{
				if (currentGroup.Parent == "Event")
					return RedirectToRoute(new { controller = "Groups", action = "GetEventsGroups", bookId = currentGroup.Book!.Id });

				return RedirectToRoute(new { controller = "Groups", action = "GetCharactersGroups", bookId = currentGroup.Book!.Id });
			}
			return RedirectToAction("Error");
		}
        //Служебные-приватные методы

        /// <summary>
        /// Проверяет, является ли владелец запроса владельцем Идеи
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<bool> UserIsGroupOwner(int groupId)
		{
            var userContentIdOwner = await _groupService.GetUserId(groupId);
            var userIdRequestOwner = Convert.ToInt32(User.FindFirst("userId")!.Value);
            if (userContentIdOwner == userIdRequestOwner)
                return true;
            return false;
        }

        public bool CheckUser(int userId)
        {
            var userIdRequestOwner = Convert.ToInt32(User.FindFirst("userId")!.Value);
            if (userId == userIdRequestOwner)
                return true;
            return false;
        }
        public async Task<bool> UserIsBookOwner(int contentId)
		{
            var userContentIdOwner = await _bookService.GetUserId(contentId);
            var userIdRequestOwner = Convert.ToInt32(User.FindFirst("userId")!.Value);
            if (userContentIdOwner == userIdRequestOwner)
                return true;
            return false;
        }
    }
}
