using Microsoft.AspNetCore.Mvc;

namespace PlotCreator.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
