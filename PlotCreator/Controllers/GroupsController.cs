using Microsoft.AspNetCore.Mvc;

namespace PlotCreator.Controllers
{
    public class GroupsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
