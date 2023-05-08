using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Helpers;
using PlotCreator.Domain.Helpers.Interfaces;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Implementations;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Controllers
{
	[Authorize]
    public class CharactersController : Controller, IInspector<bool>
    {
        private readonly ICharacterService _characterService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CharactersController(ICharacterService characterService, IWebHostEnvironment webHostEnvironment)
        {
            _characterService = characterService;
            _webHostEnvironment = webHostEnvironment;
        }
		public async Task<bool> CheckByContentId(int contentId)
        {
            var authorizedUser = Convert.ToInt32(User.FindFirst("userId")!.Value);
            var userContentIdOwner = await _characterService.GetUserId(contentId);
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
		[HttpGet]
		public async Task<IActionResult> GetCharacter(int id)
        {
			if (!CheckByContentId(id).Result)
				return View("404");

			var response = await _characterService.GetCharacter(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View(response.Data);
            return RedirectToAction("Error");
        }
		[HttpGet]
		public async Task<IActionResult> GetBookCharacters(int bookId)
        {
			//if (!CheckByContentId(bookId).Result)//Invalid?
			//	return View("404");

			ViewData["bookId"] = bookId;
			var response = await _characterService.GetBookCharacters(bookId);
            if(response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View(response.Data);
            return RedirectToAction("Error");
        }
		[HttpGet]
		public async Task<IActionResult> GetAllCharacters(int userId)
		{
			if (!CheckByUserId(userId))
				return View("404");
			var response = await _characterService.GetAllCharacters(userId);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);
			return RedirectToAction("Error");
		}
		[HttpGet]
		public async Task<IActionResult> Save(int id, int bookId, int userId)
		{
			if (!CheckByUserId(userId))
				return View("404");
			IBaseResponse<CharacterViewModel> response;
			ViewData["bookId"] = bookId;
			if (id == 0)
			{
				response = await _characterService.GetEmptyViewModel();
				response.Data.UserId = userId;
				return View(response.Data);
			}
			response = await _characterService.GetCharacter(id);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);

			return RedirectToAction("Error");
		}
		[HttpPost]
		public async Task<IActionResult> Save(CharacterViewModel model, int bookId)
		{
            if (!CheckByUserId(model.UserId))
                return View("404");
			ImageHelper imageHelper = new ImageHelper(_webHostEnvironment.WebRootPath, "Characters");
			//if (ModelState.IsValid)
			//{
            if (model.PictureImage != null)
			{
				if (model.Picture != null)
					await imageHelper.DeletePreviousImage(model.Picture);
				model.Picture = await imageHelper.SaveImage(model.PictureImage);
            }
			if (model.Id == 0)
			{
				await _characterService.CreateCharacter(model);
				if (bookId != 0)
				{
					int characterId = await _characterService.GetLastUserCharacterId(model.UserId);
					await _characterService.AddCharactersToBook(bookId, new int[] { characterId });
                    return RedirectToRoute(new { controller = "Characters", action = "GetBookCharacters", bookId = bookId });
                }

				return RedirectToRoute(new { controller = "Characters", action = "GetAllCharacters", model.UserId });
			}
			else
				await _characterService.EditCharacter(model);
			return RedirectToRoute(new { controller = "Characters", action = "GetCharacter", model.Id });
			////}
			//return RedirectToAction("Save", new { id = model.Id});
        }

		public async Task<IActionResult> Delete(int id)
		{
			var userId = await _characterService.GetUserId(id);
			var response = await _characterService.DeleteCharacter(id);
			if(response.StatusCode == Domain.Enum.StatusCode.Ok)
                return RedirectToAction($"GetAllCharacters", new { userId = userId });
            return RedirectToAction("Error");
        }
		[HttpGet]
		public async Task<IActionResult> AddCharacterToBook(int userId, int bookId)
		{
			var response = await _characterService.GetCharacterExcludeBook(userId, bookId);

			ViewData["bookId"] = bookId;
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);
			return RedirectToAction("Error");
		}
		[HttpPost]
		public async Task<IActionResult> AddCharacterToBook(int bookId, int[] characterIds)
		{
			var response = await _characterService.AddCharactersToBook(bookId, characterIds);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return RedirectToAction($"GetBookCharacters", new {bookId = bookId});
			return RedirectToAction("Error");
		}
        [HttpGet]
        public async Task<IActionResult> DeleteCharacterFromBook(int userId, int bookId)
        {
            var response = await _characterService.GetBookCharacters(bookId);

            ViewData["bookId"] = bookId;
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View(response.Data);
            return RedirectToAction("Error");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCharacterFromBook(int bookId, int[] characterIds)
        {
            var response = await _characterService.DeleteCharactersFromBook(bookId, characterIds);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return RedirectToAction($"GetBookCharacters", new { bookId = bookId });
            return RedirectToAction("Error");
        }
    }
}
