using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class Information : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
