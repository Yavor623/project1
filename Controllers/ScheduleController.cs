﻿using AccountManagement.Data;
using AccountManagement.Models;
using AccountManagement.Models.Schedules;
using AccountManagement.Models.Trains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ScheduleController(ApplicationDbContext db) => _db = db;
        public IActionResult Index(string fromWhere,string toWhere)
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
            if (!String.IsNullOrEmpty(fromWhere)&&String.IsNullOrEmpty(toWhere))
            {
                var filteredSchedules = 
                    from schedule in schedules
                    where schedule.FromWhere.ToLower().Contains(fromWhere.ToLower())
                    select schedule;
                return View(filteredSchedules);
            }
            else if (!String.IsNullOrEmpty(toWhere) && String.IsNullOrEmpty(fromWhere))
            {
                var filteredSchedules =
                    from schedule in schedules
                    where schedule.ToWhere.ToLower().Contains(toWhere.ToLower())
                    select schedule; 
                return View(filteredSchedules);
            }
            else if (!String.IsNullOrEmpty(toWhere)&& !String.IsNullOrEmpty(fromWhere))
            {
                var filteredSchedules = 
                    from schedule in schedules
                    where schedule.ToWhere.ToLower().Contains(toWhere.ToLower())
                    select schedule;
                var secondTimeFilteredSchedules =
                    from schedule in filteredSchedules
                    where schedule.FromWhere.ToLower().Contains(fromWhere.ToLower())
                    select schedule;
                return View(secondTimeFilteredSchedules);
            }
            return View(schedules);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["TrainId"] = new SelectList(_db.Trains, "Id", "SerialNumber");
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var i in _db.Schedules)
                {
                    if(model.FromWhere== i.FromWhere && (DateTime.Compare(model.StartsAtStation,i.StartsAtStation)==0 ||(( DateTime.Compare(model.StartsAtStation, i.StartsAtStation) > 0)) || DateTime.Compare(i.ArrivesAtDestination, model.StartsAtStation) < 0)){
                        ViewBag.Schedule = "Друг маршрут се изпълнява по същото време";
                        return View(model);
                    }
                    if (DateTime.Compare(model.StartsAtStation, model.ArrivesAtDestination) > 0) 
                    {
                        ViewBag.Time = "Времето на тръгване не може да е след времето на пристигане";
                        return View(model);
                    } 
                    else if (DateTime.Compare(model.ArrivesAtDestination, model.StartsAtStation) < 0)
                    {
                        ViewBag.Time = "Времето на пристигане не може да е преди времето на тръгване";
                        return View(model);
                    }
                }
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
            ViewBag.TrainId = new SelectList(_db.Trains, "Id", "SerialNumber", model.TrainId);
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewData["TrainId"] = new SelectList(_db.Trains, "Id", "SerialNumber");
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
                foreach (var i in _db.Schedules)
                {
                    if (model.FromWhere == i.FromWhere && (DateTime.Compare(model.StartsAtStation, i.StartsAtStation) == 0 || ((DateTime.Compare(model.StartsAtStation, i.StartsAtStation) > 0)) || DateTime.Compare(i.ArrivesAtDestination, model.StartsAtStation) < 0))
                    {
                        ViewBag.Schedule = "Друг маршрут се изпълнява по същото време";
                        return View(model);
                    }
                    if (DateTime.Compare(model.StartsAtStation, model.ArrivesAtDestination) > 0)
                    {
                        ViewBag.Time = "Времето на тръгване не може да е след времето на пристигане";
                        return View(model);
                    }
                    else if (DateTime.Compare(model.ArrivesAtDestination, model.StartsAtStation) < 0)
                    {
                        ViewBag.Time = "Времето на пристигане не може да е преди времето на тръгване";
                        return View(model);
                    }
                }
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
            ViewBag.TrainId = new SelectList(_db.Trains, "Id", "SerialNumber", model.TrainId);
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
