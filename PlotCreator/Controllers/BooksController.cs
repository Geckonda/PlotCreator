using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Helpers;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Controllers
{
    [Authorize]
	public class BooksController : Controller
	{
        private readonly IBookService _bookService;
        private readonly IAccountService _accountService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BooksController(IBookService bookService,
            IAccountService accountService,
            IWebHostEnvironment webHostEnvironment)
        {
            _bookService = bookService;
            _accountService = accountService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [ActionName("MyBooks")]
        public async Task<IActionResult> GetMyBooks(int userId)
        {
            if (!CheckUser(userId))
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
            if (!UserIsOwner(id).Result)
                return View("404");

            var response = await _bookService.GetBook(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View("GetMyBook", response.Data);
            return RedirectToAction("Error");
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (!UserIsOwner(id).Result)
                return View("404");

            var book = await _bookService.GetBook(id);
            var response = await _bookService.DeleteBook(id);
            await DeleteImage(book.Data.Book_cover!);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return RedirectToAction($"MyBooks", new { userId = book.Data.User!.Id });
            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Save(int id, int userId)
        {
            if (!CheckUser(userId))
                return View("404");

            IBaseResponse<BookViewModel> response;
            if (id == 0)
            {
                response = await _bookService.GetViewModel();
                response.Data.User = await _accountService.GetUser(userId);
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
            if (!CheckUser(model.User!.Id))
                return View("404");

                await SaveImage(model);
                if (model.Id == 0)
                {
                    await _bookService.CreateBook(model);
                    var book = await _bookService.GetLastUserBook(model.User.Id);
                    return RedirectToRoute(new { controller = "Books", action = "MyBook", id = book.Data.Id });
                }
                else
                {
                    await _bookService.EditBook(model);
                    return RedirectToRoute(new { controller = "Books", action = "MyBook", id = model.Id });
                }
        }


        //Служебные-приватные методы

        /// <summary>
        /// Проверяет, является ли владелец запроса владельцем Книги
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public async Task<bool> UserIsOwner(int contentId)
        {
            var userContentIdOwner = await _bookService.GetUserId(contentId);
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
    }
}
