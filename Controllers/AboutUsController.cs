using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class AboutUs : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
