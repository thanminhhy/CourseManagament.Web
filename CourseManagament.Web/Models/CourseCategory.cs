using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseManagament.Web.Models
{
    public class CourseCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string CategoryName { get; set; }
    }
}