using AccountManagement.Data;
using AccountManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Controllers
{
    public class RatingController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public RatingController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var userRatings = _db.Ratings.Include(a => a.Train).Include(a => a.User).ToList();
            var filteredRatings =
            from rate in userRatings
            where rate.UserId == _userManager.GetUserId(User)
                select rate;
            return View(filteredRatings);
        }
    }
}
