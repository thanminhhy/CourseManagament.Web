using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseManagament.Web.Models
{
    public class Trainee
    {
        [Key]
        [ForeignKey("User")]
        public string TraineeId { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        [StringLength(255)]
        public string FullName { get; set; }
        public string Email { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        [StringLength(255)]
        public string Address { get; set; }
        [Required]
        public string Education { get; set; }
    }
}