using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Helpers;
using PlotCreator.Domain.Helpers.Interfaces;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Implementations;
using PlotCreator.Service.Interfaces;
using System.IO;

namespace PlotCreator.Controllers
{

	public class BooksController : Controller, IInspector<bool>
	{
		private readonly IBookService _bookService;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public BooksController(IBookService bookService, IWebHostEnvironment webHostEnvironment)
		{
			_bookService = bookService;
			_webHostEnvironment = webHostEnvironment;
		}
		private async Task SaveImage(BookViewModel model)
		{
			ImageHelper imageHelper = new(_webHostEnvironment.WebRootPath, "BookCovers");
			if (model.Book_coverImage != null)
			{
				if (model.Book_cover != null)
				{
					await imageHelper.DeletePreviousImage(model.Book_cover);
				}
				model.Book_cover = await imageHelper.SaveImage(model.Book_coverImage);
			}
		}

		private async Task DeleteImage(string path)
		{
			ImageHelper imageHelper = new(_webHostEnvironment.WebRootPath, "BookCovers");
			await imageHelper.DeletePreviousImage(path);
		}

		public async Task<bool> CheckByContentId(int contentId)
		{
			var userContentIdOwner = await _bookService.GetUserId(contentId);
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
		[ActionName("MyBooks")]
		public async Task<IActionResult> GetMyBooks(int userId)
		{
			if (!CheckByUserId(userId))
				return View("404");

			var response = await _bookService.GetBooks(userId);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View("GetMyBooks", response.Data.ToList());
			return RedirectToAction("Error");
		}

		[HttpGet]
		[ActionName("MyBook")]
		public async Task<IActionResult> GetMyBook(int id)
		{
			if (!CheckByContentId(id).Result)
				return View("404");

			var response = await _bookService.GetBook(id);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View("GetMyBook", response.Data);
			return RedirectToAction("Error");
		}

		public async Task<IActionResult> Delete(int id)
		{
			var book = await _bookService.GetBook(id);
			var response = await _bookService.DeleteBook(id);
			await DeleteImage(book.Data.Book_cover!);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return RedirectToAction($"MyBooks", new { userId = book.Data.UserId });
			return RedirectToAction("Error");
		}

		[HttpGet]
		public async Task<IActionResult> Save(int id, int userId)
		{
			if (!CheckByUserId(userId))
				return View("404");

			IBaseResponse<BookViewModel> response;
			if (id == 0)
			{
				response = await _bookService.GetViewModel();
				response.Data.UserId = userId;
				return View(response.Data);
			}
			response = await _bookService.GetBook(id);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View(response.Data);

			return RedirectToAction("Error");
		}

		[HttpPost]
		public async Task<IActionResult> Save(BookViewModel model)
		{
			if (!CheckByUserId(model.UserId))
				return View("404");
			if (ModelState.IsValid)
			{
				await SaveImage(model);
				if (model.Id == 0)
					await _bookService.CreateBook(model);
				else
					await _bookService.EditBook(model);
				return RedirectToRoute(new { controller = "Books", action = "MyBooks", userId = model.UserId });
			}
			return RedirectToAction("Save", new { id = model.Id, userId = model.UserId });
		}

	}
}
