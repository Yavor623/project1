using AccountManagement.Data;
using AccountManagement.Models;
using AccountManagement.Models.Schedules;
using AccountManagement.Models.TrainStations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Globalization;

namespace AccountManagement.Controllers
{
    public class TrainStationController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TrainStationController(ApplicationDbContext db) => _db = db;
        public IActionResult Index()
        {
            var trainStations = _db.TrainStations.ToList();
            return View(trainStations);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainStationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var trainStation = new TrainStation
                {
                    Id = model.Id,
                    Location = model.Location
                };
                _db.TrainStations.Add(trainStation);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var theTrainStation = _db.TrainStations.FirstOrDefault(a => a.Id == id);
            var trainStation = new EditTrainStationViewModel
            {
                Id = theTrainStation.Id,
                Location = theTrainStation.Location
            };
            return View(trainStation);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditTrainStationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var trainStation = _db.TrainStations.FirstOrDefault(a => a.Id == id);

                trainStation.Id = model.Id;
                trainStation.Location = model.Location;
                _db.TrainStations.Update(trainStation);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var theTrainStation = _db.TrainStations.FirstOrDefault(a => a.Id == id);
            var trainStation = new DeleteTrainStationViewModel
            {
                Id = theTrainStation.Id,
                Location = theTrainStation.Location
            };
            return View(trainStation);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainStation = _db.TrainStations.FirstOrDefault(a => a.Id == id);
            if (trainStation != null)
            {
                _db.TrainStations.Remove(trainStation);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var theTrainStation = _db.TrainStations.FirstOrDefault(a => a.Id == id);
            if(theTrainStation.Schedules!= null) 
            {
                return View(theTrainStation);
            }
            var fakeSchedules = new List<Schedule>();
            return View(fakeSchedules);
        }
        [HttpGet]
        public IActionResult AddSchedule(int id)
        {
            ViewData["ScheduleId"] = new SelectList(_db.Schedules,"Id","FromWhere"+"-"+"ToWhere");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSchedule(int id,string selectedSchedule)
        {
            var theTrainStation = _db.TrainStations.FirstOrDefault(a => a.Id == id);
            var theSchedule = _db.Schedules.FirstOrDefault(a => a.FromWhere+"-"+a.ToWhere == selectedSchedule);
            theTrainStation.Schedules.Append(theSchedule);
            _db.TrainStations.Update(theTrainStation);
            await _db.SaveChangesAsync();
            return RedirectToAction("Details");
        }
    }
}
