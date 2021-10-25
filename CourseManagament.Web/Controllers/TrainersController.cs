using CourseManagament.Web.Models;
using CourseManagament.Web.ViewModels;
using CourseManagament.Web.Utils;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace CourseManagament.Web.Controllers
{
    [Authorize(Roles = Role.Trainer)]
    public class TrainersController : Controller
    {
        // GET: Trainers
        private ApplicationDbContext _context;
        public TrainersController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var trainerProfile = _context.Trainers
                .Where(t => t.TrainerId == userId)
                .ToList();
            return View(trainerProfile);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var userId = User.Identity.GetUserId();
            var trainerInDb = _context.Trainers.SingleOrDefault(t => t.TrainerId == id && t.TrainerId == userId);
            if (trainerInDb == null)
            {
                return HttpNotFound();
            }
            return View(trainerInDb);
        }
        [HttpPost]
        public ActionResult Edit(Trainer trainer)
        {
            var userId = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                return View(trainer);
            }
            var trainerInDb = _context.Trainers.SingleOrDefault(t => t.TrainerId == trainer.TrainerId && t.TrainerId == userId);
            if (trainerInDb == null)
            {
                return HttpNotFound();
            }
            trainerInDb.FullName = trainer.FullName;
            trainerInDb.Address = trainer.Address;
            trainerInDb.DateOfBirth = trainer.DateOfBirth;
            trainerInDb.Speciality = trainer.Speciality;
            _context.SaveChanges();
            return RedirectToAction("Index", "Trainers");
        }
        [HttpGet]
        public ActionResult AssignedCourse()
        {
            var userId = User.Identity.GetUserId();
            var courses = _context.Courses
                .Include(t => t.Category) // use this include type must have using System.Data.Entity;
                .ToList();
            var viewModels = _context.CourseTrainers
                .Where(t => t.TrainerId == userId)
                .Select(i => i.Course)
                .ToList();
            return View(viewModels);
        }
        [HttpGet]
        public ActionResult TrainnesInAssignedCourse(int id)
        {
            List<CourseTraineesViewModels> viewModels = _context.TraineeCourses
                .Where(t => t.CourseId == id)
                .GroupBy(i => i.Course)
                .Select(res => new CourseTraineesViewModels
                {
                    Course = res.Key,
                    Trainees = res.Select(i => i.Trainee).ToList()
                }).ToList();
            return View(viewModels);
        }
    }
}