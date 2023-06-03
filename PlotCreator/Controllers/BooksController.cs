using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Controllers
{
    [Authorize]
	public class BooksController : Controller
	{
        private readonly IBookService _bookService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BooksController(IBookService bookService, IWebHostEnvironment webHostEnvironment)
        {
            _bookService = bookService;
            _webHostEnvironment = webHostEnvironment;
        }
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

            var response = await _bookService.GetBook(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                return View("GetMyBook", response.Data);
            return RedirectToAction("Error");
        }

    }
}
