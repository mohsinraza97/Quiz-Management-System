using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quizilla.Models
{
    public class Question
    {
        [Key]
        [Display(Name = "Question Id")]
        public long QuestionId { get; set; }

        [Display(Name = "Quiz key")]
        [Required(ErrorMessage = "Quiz key is required.")]
        [StringLength(6)]
        public string QuizId { get; set; }

        [Display(Name = "Q.no")]
        [Required(ErrorMessage = "Q.no is required.")]
        [RegularExpression(@"[0-9]*$", ErrorMessage = "Q.no must be a number.")]
        public int Number { get; set; }

        [Display(Name = "Question")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Question is required.")]
        [AllowHtml]
        public string Description { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        [StringLength(4)]
        public string Type { get; set; }

        [Required(ErrorMessage = "Option 1 is required.")]
        [StringLength(512, MinimumLength = 1, ErrorMessage = "Option 1 cannot be longer than 512 characters.")]
        [AllowHtml]
        [Display(Name = "Option 1")]
        public string Option1 { get; set; }

        [Required(ErrorMessage = "Option 2 is required.")]
        [StringLength(512, MinimumLength = 1, ErrorMessage = "Option 2 cannot be longer than 512 characters.")]
        [AllowHtml]
        [Display(Name = "Option 2")]
        public string Option2 { get; set; }

        [StringLength(512, MinimumLength = 1, ErrorMessage = "Option 3 cannot be longer than 512 characters.")]
        [DisplayFormat(NullDisplayText = "None")]
        [AllowHtml]
        [Display(Name = "Option 3")]
        public string Option3 { get; set; }

        [StringLength(512, MinimumLength = 1, ErrorMessage = "Option 4 cannot be longer than 512 characters.")]
        [DisplayFormat(NullDisplayText = "None")]
        [AllowHtml]
        [Display(Name = "Option 4")]
        public string Option4 { get; set; }

        [Required(ErrorMessage = "Answer is required.")]
        [StringLength(512, MinimumLength = 1, ErrorMessage = "Answer cannot be longer than 512 characters.")]
        [AllowHtml]
        public string Answer { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}