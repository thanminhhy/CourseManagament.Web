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
    [Authorize(Roles = Role.TrainingStaff)]
    public class CoursesController : Controller
    {
        private ApplicationDbContext _context;
        public CoursesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Courses
        [HttpGet]
        public ActionResult Index(string searchString)
        {
            var courses = _context.Courses
                .Include(t => t.Category) // use this include type must have using System.Data.Entity;
                .ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                courses = courses
                    .Where(t => t.CourseName.ToLower().Contains(searchString.ToLower()))
                    .ToList();
            }
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

        [HttpPost]
        public ActionResult CreateCourse(CourseCategoriesViewModels model)
        {
            var viewModel = new CourseCategoriesViewModels()
            {
                Course = model.Course,
                Categories = _context.courseCategories.ToList()
            };
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            Boolean ExistCourse = _context.Courses.Any(i => i.CourseName == model.Course.CourseName);
            if (ExistCourse == true)
            {
                ModelState.AddModelError("", "Course Already Exists.");
                return View(viewModel);
            }
            var newCourse = new Course()
            {
                CourseName = model.Course.CourseName,
                DateTime = model.Course.DateTime,
                CategoryId = model.Course.CategoryId,
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

        [HttpGet]
        public ActionResult CourseTrainer(string searchString)
        {
            List<CoursetTrainersViewModels> viewModels = _context.CourseTrainers
                .GroupBy(i => i.Course)
                .Select(res => new CoursetTrainersViewModels
                {
                    Course = res.Key,
                    Trainers = res.Select(t => t.Trainer).ToList()
                })
                .ToList();

            if(!string.IsNullOrEmpty(searchString))
            {
                viewModels = viewModels
                    .Where(t => t.Course.CourseName.ToLower().Contains(searchString.ToLower()))
                    .ToList();
            }
            return View(viewModels);
        }

        [HttpGet]
        public ActionResult AddTrainer()
        {
            var viewModel = new TrainerCoursesViewModels()
            {
                Courses = _context.Courses
                .Distinct()
                .ToList(),
                Trainers = _context.Trainers
                .Distinct()
                .ToList(),
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddTrainer(TrainerCoursesViewModels model)
        {
            var viewModel = new TrainerCoursesViewModels()
            {
                Courses = _context.Courses
                .ToList(),
                Trainers = _context.Trainers
                .ToList(),
            };
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            Boolean ExistTrainer = _context.CourseTrainers.Any(i => i.TrainerId == model.TrainerId && i.CourseId == model.CourseId);
            if (ExistTrainer == true)
            {
                ModelState.AddModelError("", "Trainer Already Exists.");
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

        [HttpPost]
        public ActionResult RemoveTrainer(TrainerCoursesViewModels model)
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
            var rmtrainers = _context.CourseTrainers
                .SingleOrDefault(t => t.CourseId == model.CourseId && t.TrainerId ==model.TrainerId);
            if (rmtrainers == null)
            {
                ModelState.AddModelError("", "Trainer does not exist in this course.");
                return View(viewModel);
            }
            _context.CourseTrainers.Remove(rmtrainers);
            _context.SaveChanges();

            return RedirectToAction("CourseTrainer", "Courses");
        }
        [HttpGet]
        public ActionResult CourseTrainee(string searchString)
        {
            List<CourseTraineesViewModels> viewModels = _context.TraineeCourses
                .GroupBy(i => i.Course)
                .Select(res => new CourseTraineesViewModels
                {
                    Course = res.Key,
                    Trainees = res.Select(t => t.Trainee).ToList(),
                })
                .ToList();
            
                if(!String.IsNullOrEmpty(searchString))
                {
                viewModels = viewModels
                .Where(t => t.Course.CourseName.ToLower().Contains(searchString.ToLower()))
                .ToList();
                }
            return View(viewModels);
        }

        [HttpGet]
        public ActionResult AddTrainee()
        {
            var viewModel = new TraineeCouresesViewModels()
            {
                Courses = _context.Courses
                .Distinct()
                .ToList(),
                Trainees = _context.Trainees
                .Distinct()
                .ToList(),
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddTrainee(TraineeCouresesViewModels model)
        {
            var viewModel = new TraineeCouresesViewModels()
            {
                Courses = _context.Courses
                .ToList(),
                Trainees = _context.Trainees
                .ToList(),
            };
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            Boolean ExistTrainee = _context.TraineeCourses.Any(i => i.TraineeId == model.TraineeId && i.CourseId == model.Courseid);
            if(ExistTrainee == true)
            {
                ModelState.AddModelError("", "Trainee Already Exists.");
                return View(viewModel);
            }
            var newTrainee = new TraineeCourse()
            {
                TraineeId = model.TraineeId,
                CourseId = model.Courseid
            };
            _context.TraineeCourses.Add(newTrainee);
            _context.SaveChanges();

            return RedirectToAction("CourseTrainee", "Courses");
        }

        [HttpGet]
        public ActionResult RemoveTrainee()
        {
            var trainees = _context.TraineeCourses
                .Select(t => t.Trainee)
                .Distinct()
                .ToList();
            var courses = _context.TraineeCourses
                .Select(t => t.Course)
                .Distinct()
                .ToList();

            var viewModel = new TraineeCouresesViewModels()
            {
                Courses = courses,
                Trainees = trainees
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult RemoveTrainee(TraineeCouresesViewModels model)
        {
            var trainees = _context.TraineeCourses
                .Select(t => t.Trainee)
                .Distinct()
                .ToList();
            var courses = _context.TraineeCourses
                .Select(t => t.Course)
                .Distinct()
                .ToList();

            var viewModel = new TraineeCouresesViewModels()
            {
                Courses = courses,
                Trainees = trainees
            };
            var rmtrainees = _context.TraineeCourses
                .SingleOrDefault(t => t.CourseId == model.Courseid && t.TraineeId == model.TraineeId);
            if (rmtrainees == null)
            {
                ModelState.AddModelError("", "Trainee does not exist in this course.");
                return View(viewModel);
            }
            _context.TraineeCourses.Remove(rmtrainees);
            _context.SaveChanges();

            return RedirectToAction("CourseTrainee", "Courses");
        }
    }
}