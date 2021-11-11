using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Raspisanie.Data;
using Raspisanie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Raspisanie.Controllers
{
    public class RaspisanieController : Controller
    {
        private readonly ILogger<RaspisanieController> _logger;
        private ApplicationContext db;

        public RaspisanieController(
           ApplicationContext _db,
           ILogger<RaspisanieController> logger
           )

        {
            db = _db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Spec = db.Schedule.Include(x=>x.Specialization).Select(x => x.Specialization.Name).Distinct().ToList();
            return View();
        }
        [HttpGet]
        public IActionResult Table(string level,string trainingFormat,string specialization)
        {
            ViewBag.TrainingFormat = trainingFormat;
            ViewBag.Level = level;
            ViewBag.Specialization = specialization;
            ViewBag.Schedule = db.Schedule.Include(x => x.Lecture)
                .Include(x => x.Teachers)
                .Include(x => x.Group)
                .Include(x => x.Level)
                .Include(x => x.EducationalInstitutions)
                .Include(x => x.Time)
                .Include(x => x.Specialization)
                .Include(x => x.Group.subGroups).ToList();
            ViewBag.Days = db.Schedule.Select(x => x.Day).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> addLine(Schedule schedule,string day)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            schedule.EducationalInstitutions.Name = db.Users.FirstOrDefault(x => x.Id == userId).EducationalInstitutions;
            schedule.Faculty = db.Users.FirstOrDefault(x => x.Id == userId).Faculty;
            db.Schedule.Add(schedule);
            await db.SaveChangesAsync();
            return RedirectToAction("Raspisanie/Table");
        }
    }
}
