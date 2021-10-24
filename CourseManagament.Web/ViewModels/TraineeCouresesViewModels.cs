using CourseManagament.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagament.Web.ViewModels
{
    public class TraineeCouresesViewModels
    {
        public string TraineeId { get; set; }
        public List<Trainee> Trainees { get; set; }
        public int Courseid { get; set; }
        public List<Course> Courses { get; set; }
    }
}