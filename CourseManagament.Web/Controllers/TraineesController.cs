using CourseManagament.Web.Models;
using CourseManagament.Web.Utils;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CourseManagament.Web.ViewModels;

namespace CourseManagament.Web.Controllers
{
    [Authorize(Roles = Role.Trainee)]
    public class TraineesController : Controller
    {
        private ApplicationDbContext _context;
        public TraineesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Trainees
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var traineeProfile = _context.Trainees
                .Where(t => t.TraineeId == userId)
                .ToList();
            return View(traineeProfile);
        }

        [HttpGet]
        public ActionResult AssignedCourse()
        {
            var userId = User.Identity.GetUserId();
            var courses = _context.Courses
                .Include(t => t.Category) // use this include type must have using System.Data.Entity;
                .ToList();
            var viewModels = _context.TraineeCourses
                .Where(t => t.TraineeId == userId)
                .Select(i => i.Course)
                .ToList();
            return View(viewModels);
        }

        [HttpGet]
        public ActionResult TrainnesInAssignedCourse(int id)
        {
            var userId = User.Identity.GetUserId();
            List<CourseTraineesViewModels> viewModels = _context.TraineeCourses
                .Where(t => t.CourseId == id && t.TraineeId != userId)
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