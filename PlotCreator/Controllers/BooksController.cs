using Microsoft.AspNetCore.Mvc;

namespace PlotCreator.Controllers
{
	public class BooksController : Controller
	{
		public IActionResult MyBooks()
		{
			return View();
		}
	}
}
