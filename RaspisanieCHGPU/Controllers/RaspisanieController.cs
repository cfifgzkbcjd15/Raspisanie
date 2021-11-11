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
        public IActionResult Table(string level,string trainingFormat,string specialization,string course)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.TrainingFormat = trainingFormat;
            ViewBag.Level = level;
            ViewBag.Course = course;
            ViewBag.Specialization = specialization;
            ViewBag.EducationalInstitutions = db.Users.FirstOrDefault(x=>x.Id==userId).EducationalInstitutions;
            var user = db.Users.FirstOrDefault(x => x.Id == userId);
            ViewBag.Schedule = db.Schedule.Include(x => x.Lecture)
                .Include(x => x.Teachers)
                .Include(x => x.Group)
                .Include(x => x.Level)
                .Include(x => x.EducationalInstitutions)
                .Include(x => x.Time)
                .Include(x => x.Specialization)
                .Include(x => x.Group.subGroups)
                .Where(x=>x.EducationalInstitutions.Name==user.EducationalInstitutions&&x.Faculty==user.Faculty&&x.Specialization.Name==specialization&&x.TrainingFormat==trainingFormat&&x.Level.Name==level&&x.Course==course)
                .ToList();
            ViewBag.Days = db.Schedule.Select(x => x.Day).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> addLine(Schedule schedule)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            schedule.Faculty = db.Users.FirstOrDefault(x => x.Id == userId).Faculty;
            db.Schedule.Add(schedule);
            await db.SaveChangesAsync();
            //return RedirectToAction("Table","Raspisanie");
            return RedirectToAction("Table", "Raspisanie", new { @trainingFormat = schedule.TrainingFormat,@level=schedule.Level.Name, @specialization=schedule.Specialization.Name, @course=schedule.Course });
            //trainingFormat = Очная & level = Бакалавр & specialization = Прикладная + информатика
        }
    }
}
