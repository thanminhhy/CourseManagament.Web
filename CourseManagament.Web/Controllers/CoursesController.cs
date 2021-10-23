using CourseManagament.Web.Models;
using CourseManagament.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseManagament.Web.Controllers
{
    public class CoursesController : Controller
    {
        private ApplicationDbContext _context;
        public CoursesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Courses
        public ActionResult Index()
        {
            var courses = _context.Courses.ToList();
            return View(courses);
        }
        [HttpGet]
        public ActionResult CreateCourse()
        {
            var courseCategories = _context.courseCategories.ToList();
            var viewModel = new CourseCategoriesViewModels()
            {
                Categories = courseCategories
            };
            return View(viewModel);
        }
    }
}