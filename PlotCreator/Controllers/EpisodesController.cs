using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Implementations;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Controllers
{
    public class EpisodesController : Controller
    {
        private IEpisodeService _episodeService;
        public EpisodesController(IEpisodeService episodeService)
        {
            _episodeService= episodeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBookEpisodes(int bookId)
        {
            var response = await _episodeService.GetBookEpisodes(bookId);
            ViewData["BookId"] = bookId;
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View(response.Data);
            return RedirectToAction("Error");
        }
        [HttpGet]
        public async Task<IActionResult> GetBookEpisode(int id)
        {
            var response = await _episodeService.GetEpisode(id);
            if(response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View(response.Data);
            return RedirectToAction("Error");
        }
        [HttpGet]
        public async Task<IActionResult> Save(int id, int bookId)
        {
            IBaseResponse<EpisodeViewModel> response;
            if (id == 0)
            {
                return View(new EpisodeViewModel()
                {
                    Book = new Domain.Entity.Book
                    {
                        Id = bookId,
                    }
                });
            }
            response = await _episodeService.GetEpisode(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View(response.Data);

            return RedirectToAction("Error");
        }
        [HttpPost]
        public async Task<IActionResult> Save(EpisodeViewModel model)
        {
            if (model.Id == 0)
            {
                await _episodeService.CreateEpisode(model);
            }
            else
            {
                await _episodeService.EditEpisode(model);
            }
            return RedirectToAction($"GetBookEpisodes", new { bookId = model.Book.Id});
        }
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _episodeService.DeleteEpisode(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return RedirectToAction($"Index");
            return RedirectToAction("Error");
        }
    }
}
