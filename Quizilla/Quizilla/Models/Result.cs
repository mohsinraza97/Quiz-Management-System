using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quizilla.Models
{
    public class Result
    {
        [Key]
        [Display(Name = "Result Id")]
        public long ResultId { get; set; }

        [Required]
        [Display(Name = "Username")]
        [StringLength(20)]
        public string UserId { get; set; }

        [Display(Name = "Quiz key")]
        [Required]
        [StringLength(6)]
        public string QuizId { get; set; }

        [Display(Name = "Obtained marks")]
        [Required(ErrorMessage = "Obtained marks are required.")]
        public int ObtainedMarks { get; set; }

        public virtual Quiz Quiz { get; set; }
        public virtual User User { get; set; }
    }
}