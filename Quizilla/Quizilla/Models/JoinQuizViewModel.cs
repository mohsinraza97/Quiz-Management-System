using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quizilla.Models
{
    public class JoinQuizViewModel
    {
        [Display(Name = "Key")]
        [Required(ErrorMessage = "Key is required.")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,6}$", ErrorMessage = "Invalid key. e.g. eXsJ5i")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Key must contain only 6 characters.")]
        public string QuizId { get; set; }
    }
}