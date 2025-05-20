using AccountManagement.Data;
using AccountManagement.Models;
using AccountManagement.Models.Schedules;
using AccountManagement.Models.Trains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ScheduleController(ApplicationDbContext db) => _db = db;
        [Authorize]
        public IActionResult Index()
        {
            var schedules = _db.Schedules.Include(a => a.Train).ToList();
            foreach (var schedule in schedules)
            {
                if ((DateTime.Compare(DateTime.Now,schedule.StartsAtStation) == 0|| DateTime.Compare(DateTime.Now, schedule.StartsAtStation) > 0)&&DateTime.Compare(DateTime.Now, schedule.ArrivesAtDestination) < 0)
                {
                    schedule.Train.IsItCurrentlyUsed = true;
                }
                else
                {
                    schedule.Train.IsItCurrentlyUsed = false;
                }
            }
            return View(schedules);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["TrainId"] = new SelectList(_db.Trains, "Id", "Line");
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var schedule = new Schedule
                {
                    Id = model.Id,
                    FromWhere = model.FromWhere,
                    ToWhere = model.ToWhere,
                    StartsAtStation = model.StartsAtStation,
                    ArrivesAtDestination = model.ArrivesAtDestination,
                    TrainId = model.TrainId
                };

                _db.Schedules.Add(schedule);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainId = new SelectList(_db.Trains, "Id", "Line",model.TrainId);
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewData["TrainId"] = new SelectList(_db.Trains, "Id", "Line");
            var theSchedule = _db.Schedules.FirstOrDefault(a => a.Id == id);
            var schedule = new EditScheduleViewModel
            {
                Id = theSchedule.Id,
                FromWhere = theSchedule.FromWhere,
                ToWhere = theSchedule.ToWhere,
                StartsAtStation = theSchedule.StartsAtStation,
                ArrivesAtDestination = theSchedule.ArrivesAtDestination,
                TrainId = theSchedule.TrainId
            };

            return View(schedule);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var schedule = _db.Schedules.FirstOrDefault(a => a.Id == id);

                schedule.Id = model.Id;
                schedule.FromWhere = model.FromWhere;
                schedule.ToWhere = model.ToWhere;
                schedule.StartsAtStation = model.StartsAtStation;
                schedule.ArrivesAtDestination = model.ArrivesAtDestination;
                schedule.TrainId = model.TrainId;
                _db.Schedules.Update(schedule);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainId = new SelectList(_db.Trains, "Id", "Line", model.TrainId);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var theSchedule = _db.Schedules.FirstOrDefault(a => a.Id == id);
            ViewBag.ChosenTrain = _db.Trains.Where(a => a.Id == theSchedule.TrainId);
            var schedule = new DeleteScheduleViewModel
            {
                Id = theSchedule.Id,
                FromWhere = theSchedule.FromWhere,
                ToWhere = theSchedule.ToWhere,
                StartsAtStation = theSchedule.StartsAtStation,
                ArrivesAtDestination = theSchedule.ArrivesAtDestination,
                TrainId = theSchedule.TrainId
            };
            return View(schedule);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = _db.Schedules.FirstOrDefault(a => a.Id == id);
            if (schedule != null)
            {
                _db.Schedules.Remove(schedule);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
