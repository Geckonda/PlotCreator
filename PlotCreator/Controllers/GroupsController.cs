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

        [HttpPost]
        public async Task<IActionResult> Save(GroupViewModel newGroup)
        {
            var response = await _groupService.CreateCharacterGroup(newGroup);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return RedirectToRoute(new { controller = "Groups", action = "GetCharactersGroups", bookId = newGroup.Book!.Id });
            return RedirectToAction("Error");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id, int bookId)
        {
            var response = await _groupService.DeleteCharacterGroup(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return RedirectToRoute(new { controller = "Groups", action = "GetCharactersGroups", bookId = bookId });
            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GroupViewModel currentGroup, int bookId)
        {
            var response = await _groupService.EditCharacterGroup(currentGroup);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return RedirectToRoute(new { controller = "Groups", action = "GetCharactersGroups", bookId = currentGroup.Book!.Id });
            return RedirectToAction("Error");
        }
    }
}
