using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Helpers.Interfaces;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Implementations;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Controllers
{
    public class EventsController : Controller, IInspector<bool>
    {
        private readonly IEventService _eventService;
        private readonly IGroupService _groupService;
        public EventsController(IEventService eventService, IGroupService groupService)
        {
            _eventService = eventService;
            _groupService = groupService;
        }
        private async Task<List<Group>> GetGroups(int bookId, string parent)
        {
            var response = await _groupService.GetBookGroupsByParent(bookId, parent);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return response.Data.ToList();
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookEvents(int bookId, int userId)
        {
            if (!CheckByUserId(userId))
                return View("404");
            var response = await _eventService.GetBookEvents(bookId);
            ViewData["BookId"] = bookId;
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View(response.Data);
            return RedirectToAction("Error");
        }
        [HttpGet]
        public async Task<IActionResult> GetBookEvent(int id, int? bookId)
        {
            if (!CheckByContentId(id).Result)
                return View("404");

            ViewData["bookId"] = bookId;
            var response = await _eventService.GetEvent(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View(response.Data);
            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Save(int id, int bookId)
        {
            var response = await _eventService.GetEmptyViewModel(bookId);
            if (!CheckByUserId(response.Data.Book!.User!.Id))
                return View("404");

            ViewData["bookId"] = bookId;
            if (id == 0)
            {
                if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                    return View(response.Data);
                return RedirectToAction("Error");
            }
            response = await _eventService.GetEvent(id);
            response.Data.Groups = await GetGroups(bookId, "Event");
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View(response.Data);

            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Save(EventViewModel model)
        {
            if (!CheckByUserId(model.Book!.User!.Id))
                return View("404");

            if (model.Id == 0)
            {
                int eventId = await _eventService.GetLastUserEventId(model.Book!.Id);
                await _eventService.CreateEvent(model);
                await _eventService.AddGroupsEventRelation(eventId, model.checkedGroups);
                return RedirectToAction($"GetBookEvent", new { Id = eventId, bookId = model.Book!.Id });
            }
            else
            {
                await _eventService.EditEvent(model);
                if (model.checkedGroups == null)
                    model.checkedGroups = new int[0];
                await _eventService.EditGroupsEventRelation(model.Id, model.checkedGroups, model.Book!.Id);
                return RedirectToAction($"GetBookEvent", new { Id = model.Id, bookId = model.Book!.Id });
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (!CheckByContentId(id).Result)
                return View("404");

            var episodeResponse = await _eventService.GetEvent(id);
            if (episodeResponse.StatusCode != Domain.Enum.StatusCode.Ok)
                return RedirectToAction("Error");

            var response = await _eventService.DeleteEvent(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
            {
                return RedirectToAction($"GetBookEvents", new
                {
                    bookId = episodeResponse.Data.Book?.Id,
                    userId = episodeResponse.Data.Book?.User?.Id,
                });
            }
            return RedirectToAction("Error");
        }
        public async Task<bool> CheckByContentId(int contentId)
        {
            var authorizedUser = Convert.ToInt32(User.FindFirst("userId")!.Value);
            var userContentIdOwner = await _eventService.GetUserId(contentId);
            if (userContentIdOwner == authorizedUser)
                return true;
            return false;
        }

        public bool CheckByUserId(int userId)
        {
            var authorizedUser = Convert.ToInt32(User.FindFirst("userId")!.Value);
            if (userId == authorizedUser)
                return true;
            return false;
        }
    }
}
