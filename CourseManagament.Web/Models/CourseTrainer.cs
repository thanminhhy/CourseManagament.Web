using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseManagament.Web.Models
{
    public class CourseTrainer
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Courses")]
        public int CourseId {get;set;}
        public List<Course> Courses { get; set; }
        [Key, Column(Order = 2)]
        [ForeignKey("Trainers")]
        public string TrainerId { get; set; }
        public List<Trainer> Trainers { get; set; }
    }
}