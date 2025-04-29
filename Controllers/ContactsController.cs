using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
