using Microsoft.AspNetCore.Mvc;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Controllers
{
	public class BooksController : Controller
	{
        private readonly IBookService _bookService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BooksController(IBookService bookService, IWebHostEnvironment webHostEnvironment)
        {
            _bookService = bookService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult MyBooks()
		{
			return View();
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
