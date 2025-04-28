using AccountManagement.Data;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AccountManagement.Models.Trains;
using AccountManagement.Models;

namespace AccountManagement.Controllers
{
    public class TrainController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TrainController(ApplicationDbContext db) => _db = db;
        public IActionResult Index()
        {
            var trains = _db.Trains.Include(a => a.TypeOfTrain).ToList();
            return View(trains);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["TypeOfTrainId"] = new SelectList(_db.TypeOfTrains, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainViewModel model)
        {
            if (ModelState.IsValid)
            {
                var train = new Train
                {
                    Id = model.Id,
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
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewData["TypeOfTrainId"] = new SelectList(_db.TypeOfTrains, "Id", "Name");
            var theTrain = _db.Trains.FirstOrDefault(a => a.Id == id);
            var train = new EditTrainViewModel
            {
                Id = theTrain.Id,
                Line = theTrain.Line,
                TypeOfTrainId = theTrain.TypeOfTrainId,
                AmountOfPassagers = theTrain.AmountOfPassagers
            };

            return View(train);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditTrainViewModel model)
        {
            if (ModelState.IsValid)
            {
                var train = _db.Trains.FirstOrDefault(a => a.Id == id);

                train.Id = model.Id;
                train.Line = model.Line;
                train.TypeOfTrainId = model.TypeOfTrainId;
                train.AmountOfPassagers = model.AmountOfPassagers;
                _db.Trains.Update(train);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TypeOfTrainId = new SelectList(_db.TypeOfTrains, "Id", "Name", model.TypeOfTrainId);
            return RedirectToAction(nameof(Index));
        }
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
                AmountOfPassagers = theTrain.AmountOfPassagers
            };
            return View(train);
        }
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
    }
}
