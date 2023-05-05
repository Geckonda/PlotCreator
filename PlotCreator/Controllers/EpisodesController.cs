using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Helpers.Interfaces;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Implementations;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Controllers
{
    [Authorize]
    public class EpisodesController : Controller, IInspector<bool>
    {
        private IEpisodeService _episodeService;
        public EpisodesController(IEpisodeService episodeService)
        {
            _episodeService= episodeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBookEpisodes(int bookId, int userId)
        {
            if (!CheckByUserId(userId))
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
            if (!CheckByContentId(id).Result)
                return View("404");
            var response = await _episodeService.GetEpisode(id);
            if(response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View(response.Data);
            return RedirectToAction("Error");
        }
        [HttpGet]
        public async Task<IActionResult> Save(int id, int bookId)
        {
            var response = await _episodeService.GetEmptyViewModel(bookId);
            if (!CheckByUserId(response.Data.Book!.User!.Id))
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
            if (!CheckByUserId(model.Book!.User!.Id))
                return View("404");

            if (model.Id == 0)
                await _episodeService.CreateEpisode(model);
            else
                 await _episodeService.EditEpisode(model);
            return RedirectToAction($"GetBookEpisodes",
              new
              {
                  bookId = model.Book!.Id,
                  userId = model.Book!.User!.Id
              });
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (!CheckByContentId(id).Result)
                return View("404");
            var episodeResponse = await _episodeService.GetEpisode(id);
            var bookId = episodeResponse.Data.Book?.Id;
            var response = await _episodeService.DeleteEpisode(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok  && bookId != 0)
            {
                return RedirectToAction($"GetBookEpisodes", new { bookId = bookId });
            }
                
            return RedirectToAction("Error");
        }

        public async Task<bool> CheckByContentId(int contentId)
        {
            var authorizedUser = Convert.ToInt32(User.FindFirst("userId")!.Value);
            var userContentIdOwner = await _episodeService.GetUserId(contentId);
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
