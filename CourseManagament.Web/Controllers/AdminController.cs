﻿using CourseManagament.Web.Models;
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
            _context = new ApplicationDbContext();
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
        public ActionResult Index()
        {
            return View();
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
                var newStaff = new TrainingStaff()
                {
                    TrainingStaffId = staffId,
                    FullName = viewModel.Staff.FullName,
                    DateOfBirth = viewModel.Staff.DateOfBirth,
                    Address = viewModel.Staff.Address
                };
                var result = await UserManager.CreateAsync(user, viewModel.RegisterViewModel.Password);

                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, Role.TrainingStaff);
                    _context.TrainingStaffs.Add(newStaff);
                    _context.SaveChanges();

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
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
    }
}