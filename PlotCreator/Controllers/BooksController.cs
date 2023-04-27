using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Controllers
{
	
	public class BooksController : Controller
	{
		private readonly IBookService _bookService;
		public BooksController(IBookService bookService)
		{
			_bookService = bookService;
		}
        [HttpGet]
        [ActionName("MyBooks")]
        public async Task<IActionResult> GetMyBooks(int userId)
		{
			var response = await _bookService.GetBooks(userId);
			if (response.StatusCode == Domain.Enum.StatusCode.Ok)
				return View("GetMyBooks", response.Data.ToList());
            return RedirectToAction("Error");
        }
	}
}
