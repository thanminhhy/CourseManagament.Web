using CourseManagament.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseManagament.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext _context;
        public CategoriesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: CourseCategories
        public ActionResult Index(string searchString)
        {
            var categories = _context.courseCategories.ToList();
            if(!string.IsNullOrEmpty(searchString))
            {
                categories = categories
                    .Where(t => t.CategoryName.ToLower().Contains(searchString.ToLower()))
                    .ToList();
            }
            return View(categories);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category courseCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(courseCategory);
            }
            Boolean ExistCategory= _context.courseCategories.Any(i => i.CategoryName == courseCategory.CategoryName);
            if (ExistCategory == true)
            {
                ModelState.AddModelError("", "Course Already Exists.");
                return View(courseCategory);
            }
            var newCategory = new Category()
            {
                CategoryName = courseCategory.CategoryName
            };
            _context.courseCategories.Add(newCategory);
            _context.SaveChanges();

            return RedirectToAction("Index", "Categories");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var categoriesInDb = _context.courseCategories.SingleOrDefault(t => t.Id == id);
            if(categoriesInDb == null)
            {
                return HttpNotFound();
            }
            _context.courseCategories.Remove(categoriesInDb);
            _context.SaveChanges();

            return RedirectToAction("Index", "Categories");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var categoriesInDb = _context.courseCategories.SingleOrDefault(t => t.Id == id);
            if(categoriesInDb == null)
            {
                return HttpNotFound();
            }
            return View(categoriesInDb);
        }
        [HttpPost]
        public ActionResult Edit(Category courseCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(courseCategory);
            }
            var categoriesInDb = _context.courseCategories.SingleOrDefault(t => t.Id == courseCategory.Id);
            if(categoriesInDb == null)
            {
                return HttpNotFound();
            }
            categoriesInDb.CategoryName = courseCategory.CategoryName;
            _context.SaveChanges();

            return RedirectToAction("Index", "Categories");
        }
    }
}