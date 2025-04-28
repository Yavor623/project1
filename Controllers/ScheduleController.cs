using AccountManagement.Data;
using AccountManagement.Models;
using AccountManagement.Models.Schedules;
using AccountManagement.Models.Trains;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ScheduleController(ApplicationDbContext db) => _db = db;
        public IActionResult Index()
        {
            var schedules = _db.Schedules.Include(a => a.Train).Include(a => a.TrainStation).ToList();
            return View(schedules);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["TrainId"] = new SelectList(_db.Trains, "Id", "Line");
            ViewData["TrainStationId"] = new SelectList(_db.TrainStations, "Id", "Location");
            return View();
        }
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
                    TrainId = model.TrainId,
                    TrainStationId = model.TrainStationId
                };

                _db.Schedules.Add(schedule);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainId = new SelectList(_db.Trains, "Id", "Line",model.TrainId);
            ViewBag.TrainStationId = new SelectList(_db.Trains, "Id", "Location", model.TrainStationId);
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewData["TrainId"] = new SelectList(_db.Trains, "Id", "Line");
            ViewData["TrainStationId"] = new SelectList(_db.TrainStations, "Id", "Location");
            var theSchedule = _db.Schedules.FirstOrDefault(a => a.Id == id);
            var schedule = new EditScheduleViewModel
            {
                Id = theSchedule.Id,
                FromWhere = theSchedule.FromWhere,
                ToWhere = theSchedule.ToWhere,
                StartsAtStation = theSchedule.StartsAtStation,
                ArrivesAtDestination = theSchedule.ArrivesAtDestination,
                TrainId = theSchedule.TrainId,
                TrainStationId = theSchedule.TrainStationId
            };

            return View(schedule);
        }
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
                schedule.TrainStationId = model.TrainStationId;
                _db.Schedules.Update(schedule);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainId = new SelectList(_db.Trains, "Id", "Line", model.TrainId);
            ViewBag.TrainStationId = new SelectList(_db.Trains, "Id", "Location", model.TrainStationId);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var theSchedule = _db.Schedules.FirstOrDefault(a => a.Id == id);
            ViewBag.ChosenTrain = _db.Trains.Where(a => a.Id == theSchedule.TrainId);
            ViewBag.ChosenTrainStation = _db.TrainStations.Where(a => a.Id == theSchedule.TrainStationId);
            var schedule = new DeleteScheduleViewModel
            {
                Id = theSchedule.Id,
                FromWhere = theSchedule.FromWhere,
                ToWhere = theSchedule.ToWhere,
                StartsAtStation = theSchedule.StartsAtStation,
                ArrivesAtDestination = theSchedule.ArrivesAtDestination,
                TrainId = theSchedule.TrainId,
                TrainStationId = theSchedule.TrainStationId
            };
            return View(schedule);
        }
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
