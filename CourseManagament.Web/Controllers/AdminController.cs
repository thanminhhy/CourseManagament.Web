using CourseManagament.Web.Models;
using CourseManagament.Web.Utils;
using CourseManagament.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CourseManagament.Web.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;
        public AdminController()
        {
            _context = new ApplicationDbContext();
        }
        public AdminController(ApplicationUserManager userManager)
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
        // GET: Admin
        [HttpGet]
        public ActionResult IndexStaff(string searchString)
        {
            var trainingstaff = _context.TrainingStaffs.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                trainingstaff = trainingstaff
                    .Where(t => t.FullName.ToLower().Contains(searchString.ToLower()))
                    .ToList();
            }
            return View(trainingstaff);
        }
        [HttpGet]
        public ActionResult CreateTrainingStaff()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTrainingStaff(TrainingStaffUserViewModels viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = viewModel.RegisterViewModel.Email, Email = viewModel.RegisterViewModel.Email };
                var staffId = user.Id;
                var staffEmail = user.Email;
                var newStaff = new TrainingStaff()
                {
                    TrainingStaffId = staffId,
                    FullName = viewModel.Staff.FullName,
                    Email = staffEmail,
                    DateOfBirth = viewModel.Staff.DateOfBirth,
                    Address = viewModel.Staff.Address
                };
                var result = await UserManager.CreateAsync(user, viewModel.RegisterViewModel.Password);

                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(staffId, Role.TrainingStaff);
                    _context.TrainingStaffs.Add(newStaff);
                    _context.SaveChanges();

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("IndexStaff", "Admin");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        [HttpGet]
        public ActionResult DeleteStaff(string id)
        {
            var staffInUserDb = _context.Users.SingleOrDefault(t => t.Id == id);
            var staffInTrainingStaffDb = _context.TrainingStaffs.SingleOrDefault(t => t.TrainingStaffId == id);
            if (staffInUserDb == null || staffInTrainingStaffDb == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(staffInUserDb);
            _context.TrainingStaffs.Remove(staffInTrainingStaffDb);
            _context.SaveChanges();
            return RedirectToAction("IndexStaff", "Admin");
        }
        [HttpGet]
        public ActionResult EditStaff(string id)
        {
            var staffInDb = _context.TrainingStaffs.SingleOrDefault(t => t.TrainingStaffId == id);
            if (staffInDb == null)
            {
                return HttpNotFound();
            }
            return View(staffInDb);
        }
        [HttpPost]
        public ActionResult EditStaff(TrainingStaff trainingStaff)
        {
            if (!ModelState.IsValid)
            {
                return View(trainingStaff);
            }
            var staffInDb = _context.TrainingStaffs.SingleOrDefault(t => t.TrainingStaffId == trainingStaff.TrainingStaffId);
            if (staffInDb == null)
            {
                return HttpNotFound();
            }
            staffInDb.FullName = trainingStaff.FullName;
            staffInDb.Address = trainingStaff.Address;
            staffInDb.DateOfBirth = trainingStaff.DateOfBirth;
            _context.SaveChanges();
            return RedirectToAction("IndexStaff", "Admin");
        }

        [HttpGet]
        public ActionResult ChangeStaffPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangeStaffPassword(ChangePassWordViewModels model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var userInDb = _context.Users.SingleOrDefault(u => u.Id == model.Id);
            if(userInDb == null)
            {
                return HttpNotFound();
            }
            var userId = userInDb.Id;
            if(userId != null)
            {
                UserManager<IdentityUser> userManager =
                new UserManager<IdentityUser>(new UserStore<IdentityUser>());

                userManager.RemovePassword(userId);

                userManager.AddPassword(userId, model.Password);
                _context.SaveChanges();

                return RedirectToAction("IndexStaff", "Admin");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult IndexTrainer(string searchString)
        {
            var trainer = _context.Trainers.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                trainer = trainer
                    .Where(t => t.FullName.ToLower().Contains(searchString.ToLower()))
                    .ToList();
            }
            return View(trainer);
        }
        [HttpGet]
        public ActionResult CreateTrainer()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTrainer(TrainerUserViewModels viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = viewModel.RegisterViewModel.Email, Email = viewModel.RegisterViewModel.Email };
                var trainerId = user.Id;
                var trainerEmail = user.Email;
                var newTrainer = new Trainer()
                {
                    TrainerId = trainerId,
                    FullName = viewModel.Trainer.FullName,
                    Email = trainerEmail,
                    DateOfBirth = viewModel.Trainer.DateOfBirth,
                    Address = viewModel.Trainer.Address,
                    Speciality = viewModel.Trainer.Speciality
                };
                var result = await UserManager.CreateAsync(user, viewModel.RegisterViewModel.Password);

                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(trainerId, Role.Trainer);
                    _context.Trainers.Add(newTrainer);
                    _context.SaveChanges();


                    return RedirectToAction("IndexTrainer", "Admin");
                }
                AddErrors(result);
            }
            return View(viewModel);
        }
        [HttpGet]
        public ActionResult DeleteTrainer(string id)
        {
            var trainerInUserDb = _context.Users.SingleOrDefault(t => t.Id == id);
            var trainerInTrainerDb = _context.Trainers.SingleOrDefault(t => t.TrainerId == id);
            if (trainerInUserDb == null || trainerInTrainerDb == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(trainerInUserDb);
            _context.Trainers.Remove(trainerInTrainerDb);
            _context.SaveChanges();
            return RedirectToAction("IndexTrainer", "Admin");
        }
        [HttpGet]
        public ActionResult EditTrainer(string id)
        {
            var trainerInDb = _context.Trainers.SingleOrDefault(t => t.TrainerId == id);
            if (trainerInDb == null)
            {
                return HttpNotFound();
            }
            return View(trainerInDb);
        }
        [HttpPost]
        public ActionResult EditTrainer(Trainer trainer)
        {
            if (!ModelState.IsValid)
            {
                return View(trainer);
            }
            var trainerInDb = _context.Trainers.SingleOrDefault(t => t.TrainerId == trainer.TrainerId);
            if (trainerInDb == null)
            {
                return HttpNotFound();
            }
            trainerInDb.FullName = trainer.FullName;
            trainerInDb.Address = trainer.Address;
            trainerInDb.DateOfBirth = trainer.DateOfBirth;
            trainerInDb.Speciality = trainer.Speciality;
            _context.SaveChanges();
            return RedirectToAction("IndexTrainer", "Admin");
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ChangeTrainerPassWord()
        {
            return View();
        }
        
        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeTrainerPassword(ChangePassWordViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userInDb = _context.Users.SingleOrDefault(u => u.Id == model.Id);
            if(userInDb == null)
            {
                return HttpNotFound();
            }
            var userId = userInDb.Id;
            if (userId != null)
            {
                UserManager<IdentityUser> userManager =
                new UserManager<IdentityUser>(new UserStore<IdentityUser>());

                userManager.RemovePassword(userId);

                userManager.AddPassword(userId, model.Password);
                _context.SaveChanges();

                return RedirectToAction("IndexTrainer", "Admin");
            }

            return View(model);
        }
    }
}