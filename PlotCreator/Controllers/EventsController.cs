using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Filters;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Implementations;
using PlotCreator.Service.Interfaces;
using System.Net;

namespace PlotCreator.Controllers
{
	[Authorize]
    public class EventsController : Controller
    {
		private readonly IEventService _eventService;
		private readonly IBookService _bookService;
		private readonly IFilterService _filterService;
		public EventsController(IEventService eventService,
			IBookService bookService,
			IFilterService filterService)
		{
			_eventService = eventService;
			_bookService = bookService;
			_filterService = filterService;
		}
		[HttpGet]
		public async Task<IActionResult> GetBookEvents(int bookId, int userId)
		{
			if (!UserIsBookOwner(bookId).Result)
				return View("404");

			var response = await _eventService.GetBookEvents(bookId);
			ViewData["BookId"] = bookId;
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);
			return RedirectToAction("Error");
		}
		[HttpGet]
		public async Task<IActionResult> GetBookEvent(int id)
		{
			if (!UserIsEventOwner(id).Result)
				return View("404");

			var response = await _eventService.GetEvent(id);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);
			return RedirectToAction("Error");
		}

		[HttpGet]
		public async Task<IActionResult> Save(int id, int bookId)
		{
			var response = await _eventService.GetEmptyViewModel(bookId);
			if (!CheckUser(response.Data.Book!.User!.Id))
				return View("404");

			ViewData["bookId"] = bookId;
			if (id == 0)
			{
				if (response.StatusCode == Domain.Enum.StatusCode.Ok)
					return View(response.Data);
				return RedirectToAction("Error");
			}
			response = await _eventService.GetEvent(id);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);

			return RedirectToAction("Error");
		}

		[HttpPost]
		public async Task<IActionResult> Save(EventViewModel model)
		{
			if (!CheckUser(model.Book!.User!.Id))
				return View("404");

			if (model.Id == 0)
            {
                await _eventService.CreateEvent(model);
                var eventId = await _eventService.GetLastUserEventId(model.Book!.User!.Id!);
                return RedirectToAction($"GetBookEvent", new { id = eventId });
            }
			else
				await _eventService.EditEvent(model);

            return RedirectToAction($"GetBookEvent", new { id = model.Id });

        }
		public async Task<IActionResult> Delete(int id)
		{
			if (!UserIsEventOwner(id).Result)
				return View("404");

			var eventResponse = await _eventService.GetEvent(id);
			var bookId = eventResponse.Data.Book?.Id;
			var userId = eventResponse.Data.Book?.UserId;
			var response = await _eventService.DeleteEvent(id);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok && bookId != 0)
			{
				return RedirectToAction($"GetBookEvents", new { bookId = bookId, userId = userId });
			}
			return RedirectToAction("Error");
		}

        [HttpPost]
        public async Task<IActionResult> AddCharacters(int eventId, int[] checkedCharacters)
		{
            var response = await _eventService.AddCharactersEventRelation(eventId, checkedCharacters);
            if (response)
                return RedirectToRoute(new { controller = "Events", action = "GetBookEvent", id = eventId });
            return RedirectToAction("Error");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveCharacters(int eventId, int bookId, int[] checkedCharacters)
		{
            var response = await _eventService.EditCharactersEventRelation(eventId, checkedCharacters, bookId);
            if (response)
                return RedirectToRoute(new { controller = "Events", action = "GetBookEvent", id = eventId });
            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> AddEpisodes(int eventId, int[] checkedEpisodes)
		{
            var response = await _eventService.AddEpisodesEventRelation(eventId, checkedEpisodes);
            if (response)
                return RedirectToRoute(new { controller = "Events", action = "GetBookEvent", id = eventId });
            return RedirectToAction("Error");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveEpisodes(int eventId, int bookId, int[] checkedEpisodes)
		{
            var response = await _eventService.EditEpisodesEventRelation(eventId, checkedEpisodes, bookId);
            if (response)
                return RedirectToRoute(new { controller = "Events", action = "GetBookEvent", id = eventId });
            return RedirectToAction("Error");
        }
        [HttpPost]
        public async Task<IActionResult> AddGroups(int eventId, int bookId, int[] checkedGroups)
        {
            var response = await _eventService.AddGroupsEventRelation(eventId, checkedGroups);
            if (response)
                return RedirectToRoute(new { controller = "Events", action = "GetBookEvent", id = eventId });
            return RedirectToAction("Error");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveGroups(int eventId, int bookId, int[] checkedGroups)
        {
            var response = await _eventService.EditGroupsEventRelation(eventId, checkedGroups, bookId);
            if (response)
            {
                return RedirectToRoute(new { controller = "Events", action = "GetBookEvent", id = eventId });
            }
            return RedirectToAction("Error");
        }

		public async Task<IActionResult> FilterEvents(EventFilter eventFilter, int[] checkedGroups)
		{
			eventFilter.Groups!.CheckedObjects = checkedGroups;
			var response = await _filterService.FilterAllUserEvents(eventFilter);

			ViewData["bookId"] = eventFilter.BookId;
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
			{
				return View("GetBookEvents", response.Data);
			}
			return RedirectToAction("Error");
		}
		//Служебные-приватные методы

		public async Task<bool> UserIsEventOwner(int characterId)
		{
			var userContentIdOwner = await _eventService.GetUserId(characterId);
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
