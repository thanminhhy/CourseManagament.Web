﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseManagament.Web.Models
{
    public class TraineeCourse
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Trainee")]
        public string TraineeId { get; set; }
        public Trainee Trainee { get; set; }
        [Key, Column(Order = 2)]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}