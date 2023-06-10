using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Helpers;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Implementations;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Controllers
{
    public class CharactersController : Controller
    {
		private readonly ICharacterService _characterService;
		private readonly IBookService _bookService;
		private readonly IAccountService _accountService;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public CharactersController(ICharacterService characterService,
			IBookService bookService,
			IAccountService accountService,
			IWebHostEnvironment webHostEnvironment)
		{
			_characterService = characterService;
			_bookService = bookService;
			_accountService = accountService;
			_webHostEnvironment = webHostEnvironment;
		}

		[HttpGet]
		public async Task<IActionResult> GetCharacter(int id, int? bookId)
		{
			if (!UserIsIdeaOwner(id).Result)
				return View("404");

			ViewData["bookId"] = bookId;
			var response = await _characterService.GetCharacter(id, bookId);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);
			return RedirectToAction("Error");
		}

		[HttpGet]
		public async Task<IActionResult> GetBookCharacters(int bookId)
		{
			if (!UserIsBookOwner(bookId).Result)
				return View("404");

			ViewData["bookId"] = bookId;
			var response = await _characterService.GetBookCharacters(bookId);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);
			return RedirectToAction("Error");
		}
		[HttpGet]
		public async Task<IActionResult> GetAllCharacters(int userId)
		{
			if (!CheckUser(userId))
				return View("404");

			var response = await _characterService.GetAllCharacters(userId);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);
			return RedirectToAction("Error");
		}

		[HttpGet]
		public async Task<IActionResult> Save(int id, int bookId, int userId)
		{
			if (!CheckUser(userId))
				return View("404");

			IBaseResponse<CharacterViewModel> response;
			ViewData["bookId"] = bookId;
			if (id == 0)
			{
				response = await _characterService.GetEmptyViewModel(bookId);
				response.Data.User = await _accountService.GetUser(userId);
				return View(response.Data);
			}
			response = await _characterService.GetCharacter(id, null);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);

			return RedirectToAction("Error");
		}

		[HttpPost]
		public async Task<IActionResult> Save(CharacterViewModel model, int bookId)
		{
			if (!CheckUser(model.User!.Id))
				return View("404");

			await SaveImage(model);
			if (model.Id == 0)
			{
				await _characterService.CreateCharacter(model);
				if (bookId != 0)
				{
					int characterId = await _characterService.GetLastUserCharacterId(model.User!.Id);
					await _characterService.AddCharactersToBook(bookId, new int[] { characterId });
					//await _characterService.AddGroupsCharacterRelation(characterId, model.checkedGroups);
					return RedirectToRoute(new { controller = "Characters", action = "GetBookCharacters", bookId = bookId });
				}
				return RedirectToRoute(new { controller = "Characters", action = "GetAllCharacters", userId = model.User!.Id });
			}
			else
			{
				await _characterService.EditCharacter(model);
				if (bookId != 0)
				{
					/*if (model.checkedGroups == null)
						model.checkedGroups = new int[0];
					await _characterService.EditGroupsCharacterRelation(model.Id, model.checkedGroups, bookId);*/
					return RedirectToRoute(new { controller = "Characters", action = "GetCharacter", model.Id, bookId = bookId });
				}
				return RedirectToRoute(new { controller = "Characters", action = "GetCharacter", model.Id });
			}
		}
		public async Task<IActionResult> Delete(int id)
		{
			var userId = await _characterService.GetUserId(id);
			var characterResponse = await _characterService.GetCharacter(id, null);
			var response = await _characterService.DeleteCharacter(id);
			await DeleteImage(characterResponse.Data.Picture!);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return RedirectToAction($"GetAllCharacters", new { userId = userId });
			return RedirectToAction("Error");
		}

		[HttpGet]
		public async Task<IActionResult> AddCharacterToBook(int userId, int bookId)
		{
			var response = await _characterService.GetCharactersExcludeBook(userId, bookId);

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
				return RedirectToAction($"GetBookCharacters", new { bookId = bookId });
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

		[HttpPost]
		public async Task<IActionResult> AddGroups(int characterId, int bookId, int[] checkedGroups)
		{
			var response = await _characterService.AddGroupsCharacterRelation(characterId, checkedGroups);
			if(response)
				return RedirectToRoute(new { controller = "Characters", action = "GetCharacter", id = characterId, bookId = bookId });
			return RedirectToAction("Error");
		}
		[HttpPost]
		public async Task<IActionResult> RemoveGroups(int characterId, int bookId, int[] checkedGroups)
		{
			var response = await _characterService.EditGroupsCharacterRelation(characterId, checkedGroups, bookId);
			if (response)
			{
				ViewData["groupsMSG"] = "Группы успешно откреплены от персонажа";
				return RedirectToRoute(new { controller = "Characters", action = "GetCharacter", id = characterId, bookId = bookId });
			}
			return RedirectToAction("Error");
		}
		//Служебные-приватные методы

		/// <summary>
		/// Проверяет, является ли владелец запроса владельцем Идеи
		/// </summary>
		/// <param name="ideaId"></param>
		/// <returns></returns>
		public async Task<bool> UserIsIdeaOwner(int characterId)
		{
			var userContentIdOwner = await _characterService.GetUserId(characterId);
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
		private async Task SaveImage(CharacterViewModel model)
		{
			ImageHelper imageHelper = new ImageHelper(_webHostEnvironment.WebRootPath, "Characters");
			if (model.PictureImage != null)
			{
				if (model.Picture != null)
					await imageHelper.DeletePreviousImage(model.Picture);
				model.Picture = await imageHelper.SaveImage(model.PictureImage);
			}
		}
		private async Task DeleteImage(string path)
		{
			ImageHelper imageHelper = new(_webHostEnvironment.WebRootPath, "Characters");
			await imageHelper.DeletePreviousImage(path);
		}
	}
}
