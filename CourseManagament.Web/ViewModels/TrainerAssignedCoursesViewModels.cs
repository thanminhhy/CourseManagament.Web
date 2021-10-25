using CourseManagament.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagament.Web.ViewModels
{
    public class TrainerAssignedCoursesViewModels
    {
        public Trainer Trainer { get; set; }
        public List<Course> Courses { get; set; }
        public Course Course { get; set; }
    }
}