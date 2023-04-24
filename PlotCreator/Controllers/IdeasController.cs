using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Helpers.Interfaces;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Implementations;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Controllers
{

    [Authorize]
    public class IdeasController : Controller, IInspector<bool>
    {
        private readonly IIdeaService _ideaService;


        public IdeasController(IIdeaService ideaService)
        {
            _ideaService = ideaService;
        }

        public async Task<bool> CheckByContentId(int contentId)
        {
            var userContentIdOwner = await _ideaService.GetUserId(contentId);
            var userIdRequestOwner = Convert.ToInt32(User.FindFirst("userId")!.Value);
            if (userContentIdOwner == userIdRequestOwner)
                return true;
            return false;
        }

        public bool CheckByUserId(int userId)
        {
            var userIdRequestOwner = Convert.ToInt32(User.FindFirst("userId")!.Value); 
            if (userId == userIdRequestOwner)
                return true;
            return false;
        }

        [HttpGet]
        [ActionName("MyIdeas")]
        public async Task <IActionResult> GetIdeas(int id)
        {
            if (!CheckByUserId(id))
                return View("404");

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
            if(!CheckByContentId(id).Result)
                return View("404");

            var response = await _ideaService.GetIdea(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View("GetIdea", response.Data);
            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Save(int id, int userId)
        {
            if (!CheckByUserId(userId))
                return View("404");

            if (id == 0)
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
            if (!CheckByUserId(model.UserId))
                return View("404");

            if (ModelState.IsValid)
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
