using AccountManagement.Data;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AccountManagement.Models.Trains;
using AccountManagement.Models;
using Microsoft.AspNetCore.Authorization;
using AccountManagement.Models.Rating;
using Microsoft.AspNetCore.Identity;

namespace AccountManagement.Controllers
{
    public class TrainController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public TrainController(ApplicationDbContext db,UserManager<ApplicationUser> userManager) 
        {
            _db = db; 
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var trains = _db.Trains.Include(a => a.TypeOfTrain).Include(a => a.Schedules).ToList();
            if (trains.Count!= 0)
            {
                foreach (var train in trains)
                {
                    if (train.Schedules.Count()!=0)
                    {
                        foreach (var trainSchedule in train.Schedules)
                        {
                            if ((DateTime.Compare(DateTime.Now, trainSchedule.StartsAtStation) == 0 || DateTime.Compare(DateTime.Now, trainSchedule.StartsAtStation) > 0) && DateTime.Compare(DateTime.Now, trainSchedule.ArrivesAtDestination) < 0)
                            {
                                train.IsItBusy = true;
                            }
                            else
                            {
                                train.IsItBusy = false;
                            }
                        }
                    }
                    
                }
            }
            return View(trains);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["TypeOfTrainId"] = new SelectList(_db.TypeOfTrains, "Id", "Name");
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainViewModel model)
        {
            if (ModelState.IsValid)
            {
                var train = new Train
                {
                    Id = model.Id,
                    SerialNumber = model.SerialNumber,
                    Line = model.Line,
                    TypeOfTrainId = model.TypeOfTrainId,
                    AmountOfPassagers = model.AmountOfPassagers
                };
                _db.Trains.Add(train);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new SelectList(_db.TypeOfTrains, "Id", "Name", model.TypeOfTrainId);
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewData["TypeOfTrainId"] = new SelectList(_db.TypeOfTrains, "Id", "Name");
            var theTrain = _db.Trains.FirstOrDefault(a => a.Id == id);
            var train = new EditTrainViewModel
            {
                Id = theTrain.Id,
                SerialNumber = theTrain.SerialNumber,
                Line = theTrain.Line,
                TypeOfTrainId = theTrain.TypeOfTrainId,
                AmountOfPassagers = theTrain.AmountOfPassagers
            };

            return View(train);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditTrainViewModel model)
        {
            if (ModelState.IsValid)
            {
                var train = _db.Trains.FirstOrDefault(a => a.Id == id);

                train.Id = model.Id;
                train.Line = model.Line;
                train.SerialNumber = model.SerialNumber;
                train.TypeOfTrainId = model.TypeOfTrainId;
                train.AmountOfPassagers = model.AmountOfPassagers;
                _db.Trains.Update(train);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TypeOfTrainId = new SelectList(_db.TypeOfTrains, "Id", "Name", model.TypeOfTrainId);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {

            var theTrain = _db.Trains.FirstOrDefault(a => a.Id == id);
            ViewBag.ChosenTypeOfTrain = _db.TypeOfTrains.Where(a => a.Id == theTrain.TypeOfTrainId);
            var train = new DeleteTrainViewModel
            {
                Id = theTrain.Id,
                Line = theTrain.Line,
                TypeOfTrainId = theTrain.TypeOfTrainId,
                SerialNumber = theTrain.SerialNumber,
                AmountOfPassagers = theTrain.AmountOfPassagers
            };
            return View(train);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var train = _db.Trains.FirstOrDefault(a => a.Id == id);
            if (train != null)
            {
                _db.Trains.Remove(train);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        [HttpGet]
        public IActionResult Rate(int id)
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Rate(int id,CreateRatingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var rating = new Assesment
                {
                    RatingScore = model.RatingScore,
                    Comment = model.Comment,
                    TrainId = id,
                    UserId = _userManager.GetUserId(User),
                    TimeItWasAdded = DateTime.Now
                };
                _db.Ratings.Add(rating);
                _db.SaveChanges();

                decimal ratingScoreSum = 0;
                var ratings = _db.Ratings.Where(a => a.TrainId == id);
                foreach (var rate in ratings)
                {
                    ratingScoreSum += rate.RatingScore;
                }
                decimal ratingScore = Math.Round((ratingScoreSum/ratings.Count()),1);
                var train = _db.Trains.FirstOrDefault(a => a.Id == id);
                train.RatingScore = $"{ratingScore}";
                _db.Trains.Update(train);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
