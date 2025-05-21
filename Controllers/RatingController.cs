using AccountManagement.Data;
using AccountManagement.Models;
using AccountManagement.Models.Rating;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IActionResult Index()
        {
            var userRatings = _db.Ratings.Include(a => a.Train).Include(a => a.User).ToList();
            var filteredRatings =
            from rate in userRatings
            where rate.UserId == _userManager.GetUserId(User)
                select rate;
            return View(filteredRatings);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var theRating = _db.Ratings.FirstOrDefault(a => a.Id == id);
            var rating = new EditRatingViewModel
            {
                Id = theRating.Id,
                RatingScore = theRating.RatingScore,
                Comment = theRating.Comment,
                TimeItWasAdded = DateTime.Now,
                IsItEdited = true
            };

            return View(rating);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRatingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var rating = _db.Ratings.FirstOrDefault(a => a.Id == id);

                rating.Id = model.Id;
                rating.RatingScore = model.RatingScore;
                rating.Comment = model.Comment;
                rating.TimeItWasAdded = model.TimeItWasAdded;
                rating.IsItEdited = true;
                _db.Ratings.Update(rating);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var theRating = _db.Ratings.FirstOrDefault(a => a.Id == id);
            ViewBag.ChosenTrain = _db.Trains.Where(a => a.Id == theRating.TrainId);
            var rating = new DeleteRatingViewModel
            {
                Id = theRating.Id,
                RatingScore = theRating.RatingScore,
                Comment = theRating.Comment,
                TrainId = theRating.TrainId,
                UserId = _userManager.GetUserId(User),
                TimeItWasAdded = DateTime.Now,
            };
            return View(rating);
        }
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rating = _db.Ratings.FirstOrDefault(a => a.Id == id);
            if (rating != null)
            {
                var rates = _db.Ratings.Where(a => a.TrainId == rating.TrainId);
                decimal ratingScoreSum = 0;
                foreach(var rate in rates)
                {
                    if (rate.Id != id)
                    {
                        ratingScoreSum += rate.RatingScore;
                    }
                }
                decimal finalScore = Math.Round((ratingScoreSum / rates.Count()), 1);
                var train = _db.Trains.FirstOrDefault(a => a.Id == rating.TrainId);
                if (finalScore<=0)
                {
                    train.RatingScore = "Unrated";
                }
                else
                {
                    train.RatingScore = $"{finalScore}";
                }
                _db.Ratings.Remove(rating);
                _db.Trains.Update(train);
                
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
