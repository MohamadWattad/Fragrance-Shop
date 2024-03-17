using Microsoft.AspNetCore.Mvc;

namespace MvcProject99.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }
    }
}
