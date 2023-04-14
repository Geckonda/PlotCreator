using Microsoft.AspNetCore.Mvc;

namespace PlotCreator.Controllers
{
    public class IdeasController : Controller
    {
        public IActionResult MyIdeas()
        {
            return View();
        }
    }
}
