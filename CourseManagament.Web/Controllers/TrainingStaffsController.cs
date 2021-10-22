using CourseManagament.Web.Models;
using CourseManagament.Web.Utils;
using CourseManagament.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CourseManagament.Web.Controllers
{
    [Authorize(Roles = Role.TrainingStaff)]
    public class TrainingStaffsController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;
        public TrainingStaffsController()
        {
            _context = new ApplicationDbContext();
        }
        public TrainingStaffsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        // GET: TrainingStaff
        public ActionResult IndexTrainee()
        {
            var trainee = _context.Trainees.ToList();
            return View(trainee);
        }
        [HttpGet]
        public ActionResult CreateTrainee()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTrainee(TraineeUserViewModels viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = viewModel.RegisterViewModel.Email, Email = viewModel.RegisterViewModel.Email };
                var traineeId = user.Id;
                var traineeEmail = user.Email;
                var newTrainee = new Trainee()
                {
                    TraineeId = traineeId,
                    FullName = viewModel.Trainee.FullName,
                    Email = traineeEmail,
                    DateOfBirth = viewModel.Trainee.DateOfBirth,
                    Address = viewModel.Trainee.Address,
                    Education = viewModel.Trainee.Education
                };
                var result = await UserManager.CreateAsync(user, viewModel.RegisterViewModel.Password);

                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(traineeId, Role.Trainee);
                    _context.Trainees.Add(newTrainee);
                    _context.SaveChanges();

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("IndexTrainee", "TrainingStaffs");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }
        [HttpGet]
        public ActionResult DeleteTrainee(string id)
        {
            var traineeInDb = _context.Trainees.SingleOrDefault(t => t.TraineeId == id);
            var traineeInUserDb = _context.Users.SingleOrDefault(t => t.Id == id);
            if(traineeInDb == null || traineeInUserDb == null)
            {
                return HttpNotFound();
            }
            _context.Trainees.Remove(traineeInDb);
            _context.Users.Remove(traineeInUserDb);
            _context.SaveChanges();

            return RedirectToAction("IndexTrainee", "TrainingStaffs");
        }
        [HttpGet]
        public ActionResult EditTrainee(string id)
        {
            var traineeInDb = _context.Trainees.SingleOrDefault(t => t.TraineeId == id);
            if(traineeInDb == null)
            {
                return HttpNotFound();
            }
            return View(traineeInDb);
        }
        [HttpPost]
        public ActionResult EditTrainee(Trainee trainee)
        {
            if (!ModelState.IsValid) 
            {
                return View(trainee);
            }
            var traineeInDb = _context.Trainees.SingleOrDefault(t => t.TraineeId == trainee.TraineeId);
            if(traineeInDb == null)
            {
                return HttpNotFound();
            }
            traineeInDb.FullName = trainee.FullName;
            traineeInDb.DateOfBirth = trainee.DateOfBirth;
            traineeInDb.Address = trainee.Address;
            traineeInDb.Education = trainee.Education;
            _context.SaveChanges();

            return RedirectToAction("IndexTrainee", "TrainingStaffs");
        }
    }
}