using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quizilla.Models
{
    public class Course
    {
        [Key]
        [Display(Name = "Course Id")]
        public long CourseId { get; set; }
        
        [Required(ErrorMessage = "Code is required.")]
        [RegularExpression("^[A-Z+]{3}-[0-9+]{3}$", ErrorMessage = "Invalid code. e.g. SEN-310")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "Code must contain only 7 characters.")]
        //[Remote("ValidateCode", "Course", ErrorMessage = "This code is already assigned to some other course. Please try another.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [RegularExpression(@"^[a-zA-Z&\s]{1,50}$", ErrorMessage = "Invalid title. e.g. Data Communication & Networking")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Title cannot be longer than 50 characters.")]
        public string Title { get; set; }

        [Display(Name = "Credit hour")]
        [Required(ErrorMessage = "Credit hour is required.")]
        [RegularExpression(@"[0-9]*$", ErrorMessage = "Credit hour must be a number.")]
        public int Credits { get; set; }

        [Required]
        [Display(Name = "Username")]
        [StringLength(20)]
        public string UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
    }
}