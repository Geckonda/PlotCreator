using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlotCreator.Controllers
{
	
	public class BooksController : Controller
	{
        [HttpGet]
        public IActionResult MyBooks()
		{
			return View("MyBooks");
		}
	}
}
