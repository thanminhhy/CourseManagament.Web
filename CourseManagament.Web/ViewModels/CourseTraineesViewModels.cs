using CourseManagament.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagament.Web.ViewModels
{
    public class CourseTraineesViewModels
    {
        public Course Course { get; set; }
        public List<Trainee> Trainees { get; set; }
    }
}