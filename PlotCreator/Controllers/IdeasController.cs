using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Implementations;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Controllers
{
    
    public class IdeasController : Controller
    {
        private readonly IIdeaService _ideaService;

        public IdeasController(IIdeaService ideaService)
        {
            _ideaService = ideaService;
        }

        [HttpGet]
        [ActionName("MyIdeas")]
        public async Task <IActionResult> GetIdeas(int id)
        {
           var response = await _ideaService.GetIdeas(id);
            if(response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View("GetIdeas", response.Data.ToList());
            if(response.StatusCode == Domain.Enum.StatusCode.NotFound)
                return RedirectToAction("Save", "Ideas",
                    new { id = 0, UserId = id });
            return RedirectToAction("Error");
        }

        [ActionName("MyIdea")]
        public async Task<IActionResult> GetIdea(int id)
        {
            var response = await _ideaService.GetIdea(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View("GetIdea", response.Data);
            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Save(int id, int userId)
        {
            if(id == 0)
            {
                var newIdea = new IdeaViewModel()
                {
                    UserId = userId,
                };
                return View(newIdea);
            }
            var response = await _ideaService.GetIdea(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View(response.Data);

            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Save(IdeaViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(model.Id == 0)
                {
                    await _ideaService.CreateIdea(model);
                }
                else
                {
                    await _ideaService.EditIdea(model.Id, model);
                }
            }
            return RedirectToRoute(new { controller = "Ideas", action = "MyIdeas", id = model.UserId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = await _ideaService.GetUserId(id);
            var response = await _ideaService.DeleteIdea(id);
            if(response.StatusCode == Domain.Enum.StatusCode.Ok)
                return RedirectToAction($"MyIdeas", new { id = userId });
            return RedirectToAction("Error");
        }
    }
}
