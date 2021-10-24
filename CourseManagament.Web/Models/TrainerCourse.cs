﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseManagament.Web.Models
{
    public class TrainerCourse
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Course")]
        public int CourseId {get;set;}
        public Course Course { get; set; }
        [Key, Column(Order = 2)]
        [ForeignKey("Trainer")]
        public string TrainerId { get; set; }
        public Trainer Trainer { get; set; }
    }
}