﻿using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CourseManagament.Web.Models
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> courseCategories { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<TrainingStaff> TrainingStaffs { get; set; }
        public DbSet<CourseTrainer> CourseTrainers { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}