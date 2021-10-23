using CourseManagament.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagament.Web.ViewModels
{
    public class CourseTrainerViewModels
    {
        public int CourseId { get; set; }
        public List<Course> Courses { get; set; }
        public string TrainerId { get; set; }
        public List<Trainer> Trainers { get; set; }
    }
}