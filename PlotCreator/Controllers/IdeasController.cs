using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Implementations;
using PlotCreator.Service.Interfaces;
using System.Net;

namespace PlotCreator.Controllers
{
    [Authorize]
    public class IdeasController : Controller
    {
        private readonly IIdeaService _ideaService;
        private readonly IAccountService _accountService;
        private readonly IBookService _bookService;
        public IdeasController(IIdeaService ideaService,
            IAccountService accountService,
            IBookService bookService)
        {
            _ideaService = ideaService;
            _accountService = accountService;
            _bookService = bookService;
        }

        [HttpGet]
        [ActionName("MyIdeas")]
        public async Task<IActionResult> GetIdeas(int userId)
        {
            if (!CheckUser(userId))
                return View("404");


            var response = await _ideaService.GetIdeas(userId);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View("GetIdeas", response.Data.ToList()); ;
            return RedirectToAction("Error");
        }

        [HttpGet]
        [ActionName("MyIdea")]
        public async Task<IActionResult> GetIdea(int id, int? bookId)
        {
            if (!UserIsIdeaOwner(id).Result)
                return View("404");

            if (bookId != null)
                ViewData["bookId"] = bookId;

            var response = await _ideaService.GetIdea(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View("GetIdea", response.Data);
            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> GetBookIdeas(int bookId)
        {
            if (!UserIsBookOwner(bookId).Result)
                return View("404");

            ViewData["bookId"] = bookId;
            var response = await _ideaService.GetBookIdeas(bookId);
            var book = await _ideaService.GetBook(bookId);
            ViewData["bookTitle"] = book.Title;
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View("GetBookIdeas", response.Data);
            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Save(int id, int bookId, int userId)
        {
            if (!CheckUser(userId))
                return View("404");

            ViewData["bookId"] = bookId;
            if (id == 0)
            {
                var user = await _accountService.GetUser(userId);
                var book = await _ideaService.GetBook(bookId);
                var newIdea = new IdeaViewModel()
                {
                    User = user,
                    Books = new() { book },
                };
                return View(newIdea);
            }
            var response = await _ideaService.GetIdea(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View(response.Data);

            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Save(IdeaViewModel model, int bookId)
        {
            if (!CheckUser(model.User!.Id))
                return View("404");

            if (model.Id == 0)
            {
                await _ideaService.CreateIdea(model);
                var response = await _ideaService.GetLastUserIdea(model.User!.Id);
                if (bookId != 0)
                {
                    await _ideaService.AddIdeasToBook(bookId, new int[] { response.Data.Id });
                    return RedirectToRoute(new { controller = "Ideas", action = "GetBookIdeas", bookId = bookId });
                }
                return RedirectToRoute(new { controller = "Ideas", action = "MyIdea", id = response.Data.Id });
            }
            else
            {
                await _ideaService.EditIdea(model.Id, model);
                return RedirectToRoute(new { controller = "Ideas", action = "MyIdea", id = model.Id });
            }
        }


        public async Task<IActionResult> Delete(int id, int? bookId)
        {
            var userId = await _ideaService.GetUserId(id);
            var response = await _ideaService.DeleteIdea(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
            {
                if(bookId != null)
                    return RedirectToAction($"GetBookIdeas", new { bookId = bookId });
                return RedirectToAction($"MyIdeas", new { userId = userId });
            }
            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> AddIdeasToBook(int userId, int bookId)
        {
            var response = await _ideaService.GetIdeaExcludeBook(userId, bookId);

            ViewData["bookId"] = bookId;
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View(response.Data);
            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> AddIdeasToBook(int bookId, int[] ideaIds)
        {
            var response = await _ideaService.AddIdeasToBook(bookId, ideaIds);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return RedirectToAction($"GetBookIdeas", new { bookId = bookId });
            return RedirectToAction("Error");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteIdeasFromBook(int userId, int bookId)
        {
            var response = await _ideaService.GetBookIdeas(bookId);

            ViewData["bookId"] = bookId;
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View(response.Data);
            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIdeasFromBook(int bookId, int[] ideaIds)
        {
            var response = await _ideaService.DeleteIdeasFromBook(bookId, ideaIds);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return RedirectToAction($"GetBookIdeas", new { bookId = bookId });
            return RedirectToAction("Error");
        }

        //Служебные-приватные методы

        /// <summary>
        /// Проверяет, является ли владелец запроса владельцем Идеи
        /// </summary>
        /// <param name="ideaId"></param>
        /// <returns></returns>
        public async Task<bool> UserIsIdeaOwner(int ideaId)
        {
            var userContentIdOwner = await _ideaService.GetUserId(ideaId);
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
