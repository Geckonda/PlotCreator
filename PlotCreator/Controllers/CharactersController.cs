using Microsoft.AspNetCore.Mvc;

namespace PlotCreator.Controllers
{
    public class CharactersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
