using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IGroupService _groupService;
        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCharactersGroups(int bookId)
        {
            var response =  await _groupService.GetAllGroupsByParent(bookId, "Character");
            ViewData["BookId"] = bookId;
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View(response.Data);
            return RedirectToAction("Error");
        }
        [HttpGet]
        public async Task<IActionResult> GetEventsGroups(int bookId)
        {
            var response = await _groupService.GetAllGroupsByParent(bookId, "Event");
            ViewData["BookId"] = bookId;
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
                if(newGroup.Parent == "Event")
                    return RedirectToRoute(new { controller = "Groups", action = "GetEventsGroups", bookId = newGroup.Book!.Id });

                return RedirectToRoute(new { controller = "Groups", action = "GetCharactersGroups", bookId = newGroup.Book!.Id });
            }
            return RedirectToAction("Error");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id, int bookId, string parent)
        {
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
    }
}
