using CourseManagament.Web.Models;
using CourseManagament.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CourseManagament.Web.Utils;

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
        [HttpGet]
        public ActionResult Index()
        {
            var courses = _context.Courses
                .Include(t => t.Category) // use this include type must have using System.Data.Entity;
                .ToList();
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
        public ActionResult CreateCourse(CourseCategoriesViewModels model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CourseCategoriesViewModels()
                {
                    Course = model.Course,
                    Categories = _context.courseCategories.ToList()
                };
                return View(viewModel);
            }
            var newCourse = new Course()
            {
                CourseName = model.Course.CourseName,
                DateTime = model.Course.DateTime,
                CategoryId = model.Course.CategoryId
            };
            _context.Courses.Add(newCourse);
            _context.SaveChanges();

            return RedirectToAction("Index", "Courses");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var courseInDb = _context.Courses.SingleOrDefault(t => t.Id == id);
            if (courseInDb == null)
            {
                return HttpNotFound();
            }
            _context.Courses.Remove(courseInDb);
            _context.SaveChanges();

            return RedirectToAction("Index", "Courses");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var courseInDb = _context.Courses.SingleOrDefault(t => t.Id == id);
            if (courseInDb == null)
            {
                return HttpNotFound();
            }
            var viewModel = new CourseCategoriesViewModels()
            {
                Course = courseInDb,
                Categories = _context.courseCategories.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Edit(CourseCategoriesViewModels model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CourseCategoriesViewModels()
                {
                    Course = model.Course,
                    Categories = _context.courseCategories.ToList()
                };
                return View(viewModel);
            }
            var courseInDb = _context.Courses.SingleOrDefault(t => t.Id == model.Course.Id);
            if(courseInDb == null)
            {
                return HttpNotFound();
            }
            courseInDb.CourseName = model.Course.CourseName;
            courseInDb.DateTime = model.Course.DateTime;
            courseInDb.CategoryId = model.Course.CategoryId;

            _context.SaveChanges();
            return RedirectToAction("Index", "Courses");
        }
        public ActionResult CourseTrainer()
        {
            List<CoursetTrainersViewModels> viewModels = _context.CourseTrainers
                .GroupBy(i => i.Course)
                .Select(res => new CoursetTrainersViewModels
                {
                    Course = res.Key,
                    Trainers = res.Select(t => t.Trainer).ToList()
                })
                .ToList();
            return View(viewModels);
        }
        [HttpGet]
        public ActionResult AddTrainer()
        {
            var viewModel = new TrainerCoursesViewModels()
            {
                Courses = _context.Courses.ToList(),
                Trainers = _context.Trainers.ToList(),
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult AddTrainer(TrainerCoursesViewModels model)
        {
            if(!ModelState.IsValid)
            {
                var viewModel = new TrainerCoursesViewModels()
                {
                    Courses = _context.Courses.ToList(),
                    Trainers = _context.Trainers.ToList(),
                };
                return View(viewModel);
            }
            var newmodel = new TrainerCourse()
            {
                CourseId = model.CourseId,
                TrainerId = model.TrainerId
            };
            _context.CourseTrainers.Add(newmodel);
            _context.SaveChanges();

            return RedirectToAction("CourseTrainer", "Courses");
        }
        [HttpGet]
        public ActionResult RemoveTrainer()
        {
            var trainers = _context.CourseTrainers
                .Select(t => t.Trainer)
                .Distinct()
                .ToList();
            var courses = _context.CourseTrainers
                .Select(t => t.Course)
                .Distinct()
                .ToList();

            var viewModel = new TrainerCoursesViewModels()
            {
                Courses = courses,
                Trainers = trainers
            };
            return View(viewModel);
        }
    }
}