using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Implementations;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Controllers
{
	[Authorize]
    public class EpisodesController : Controller
    {

        private readonly IEpisodeService _episodeService;
        private readonly IBookService _bookService;
        private readonly IAccountService _accountService;
        public EpisodesController(IEpisodeService episodeService,
            IBookService bookService,
            IAccountService accountService)
        {
            _episodeService = episodeService;
            _bookService = bookService;
            _accountService = accountService;
        }

		[HttpGet]
		public async Task<IActionResult> GetBookEpisodes(int bookId, int userId)
		{
			if (!UserIsBookOwner(bookId).Result)
				return View("404");
			var response = await _episodeService.GetBookEpisodes(bookId);
			ViewData["BookId"] = bookId;
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);
			return RedirectToAction("Error");
		}
		[HttpGet]
		public async Task<IActionResult> GetBookEpisode(int id)
		{
			if (!UserIsEpisodeOwner(id).Result)
				return View("404");
			var response = await _episodeService.GetEpisode(id);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);
			return RedirectToAction("Error");
		}
		[HttpGet]
		public async Task<IActionResult> Save(int id, int bookId)
		{
			var response = await _episodeService.GetEmptyViewModel(bookId);
			if (!CheckUser(response.Data.Book!.User!.Id))
				return View("404");
			if (id == 0)
			{
				if (response.StatusCode == Domain.Enum.StatusCode.Ok)
					return View(response.Data);
				return RedirectToAction("Error");
			}
			response = await _episodeService.GetEpisode(id);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);

			return RedirectToAction("Error");
		}
		[HttpPost]
		public async Task<IActionResult> Save(EpisodeViewModel model)
		{
			if (!CheckUser(model.Book!.User!.Id))
				return View("404");

			if (model.Id == 0)
				await _episodeService.CreateEpisode(model);
			else
				await _episodeService.EditEpisode(model);
			var episodeId = _episodeService.GetLastUserEpisodeId(model.Book!.User!.Id!);
			return RedirectToAction($"GetBookEpisode",
			  new
			  {
                  id = episodeId.Result
              });
		}
		public async Task<IActionResult> Delete(int id)
		{
			if (!UserIsEpisodeOwner(id).Result)
				return View("404");
			var episodeResponse = await _episodeService.GetEpisode(id);
			var bookId = episodeResponse.Data.Book?.Id;
			var userId = episodeResponse.Data.Book?.UserId;
			var response = await _episodeService.DeleteEpisode(id);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok && bookId != 0)
			{
				return RedirectToAction($"GetBookEpisodes", new { bookId = bookId, userId = userId });
			}

			return RedirectToAction("Error");

		}
		[HttpPost]
		public async Task<IActionResult> AddCharacters(int episodeId, int[] checkedCharacters)
		{
			var response = await _episodeService.AddCharactersEpisodeRelation(episodeId, checkedCharacters);
			if (response)
				return RedirectToRoute(new { controller = "Episodes", action = "GetBookEpisode", id = episodeId });
			return RedirectToAction("Error");
		}
		[HttpPost]
		public async Task<IActionResult> RemoveCharacters(int episodeId, int bookId, int[] checkedCharacters)
		{
			var response = await _episodeService.EditCharactersEpisodeRelation(episodeId, checkedCharacters, bookId);
			if (response)
				return RedirectToRoute(new { controller = "Episodes", action = "GetBookEpisode", id = episodeId });
			return RedirectToAction("Error");
		}

		[HttpPost]
		public async Task<IActionResult> AddEvents(int episodeId, int[] checkedEvents)
		{
			var response = await _episodeService.AddEventsEpisodeRelation(episodeId, checkedEvents);
			if (response)
				return RedirectToRoute(new { controller = "Episodes", action = "GetBookEpisode", id = episodeId });
			return RedirectToAction("Error");
		}
		[HttpPost]
		public async Task<IActionResult> RemoveEvents(int episodeId, int bookId, int[] checkedEvents)
		{
			var response = await _episodeService.EditEventsEpisodeRelation(episodeId, checkedEvents, bookId);
			if (response)
				return RedirectToRoute(new { controller = "Episodes", action = "GetBookEpisode", id = episodeId });
			return RedirectToAction("Error");
		}

		//Служебные-приватные методы

		public async Task<bool> UserIsEpisodeOwner(int characterId)
		{
			var userContentIdOwner = await _episodeService.GetUserId(characterId);
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
