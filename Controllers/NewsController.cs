using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Controllers
{
	public class NewsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
