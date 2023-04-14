using Microsoft.AspNetCore.Mvc;

namespace PlotCreator.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        public IActionResult Register() => View();



        [HttpGet]
        public IActionResult Login() => View();
    }
}
