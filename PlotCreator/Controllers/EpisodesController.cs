using Microsoft.AspNetCore.Mvc;

namespace PlotCreator.Controllers
{
    public class EpisodesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
