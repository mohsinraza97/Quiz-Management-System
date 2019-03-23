using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quizilla.Models
{
    public class Quiz
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Key")]
        [Required(ErrorMessage = "Key is required.")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,6}$", ErrorMessage = "Invalid key. e.g. eXsJ5i")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Key must contain only 6 characters.")]
        [Remote("ValidateQuizId", "Quiz", ErrorMessage = "This key is already assigned to some other quiz. Please try another.")]
        public string QuizId { get; set; }

        [Display(Name = "Course")]
        [Required(ErrorMessage = "Course is required.")]
        public long CourseId { get; set; }

        [Required]
        [Display(Name = "Username")]
        [StringLength(20)]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Title cannot be longer than 50 characters.")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Duration is required.")]
        [RegularExpression(@"[0-9]*$", ErrorMessage = "Duration must be a number.")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        //[DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd-MMM-yyyy}")]
        //[RegularExpression("^[0-9+]{2}/[0-9+]{2}/[0-9+]{4}$", ErrorMessage = "Invalid date. e.g. 04/02/2018")]
        public DateTime Date { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayFormat(NullDisplayText = "None")]
        public string Instructions { get; set; }

        [Display(Name = "Passing marks")]
        [Required(ErrorMessage = "Passing marks are required.")]
        [RegularExpression(@"[0-9]*$", ErrorMessage = "Passing marks must be a number.")]
        [Range(1, 100, ErrorMessage = "Passing marks must be between 1 and 100")]
        public int PassingMarks { get; set; }

        [Display(Name = "Total marks")]
        [Required(ErrorMessage = "Total marks are required.")]
        [Range(1, 100, ErrorMessage = "Total marks must be between 1 and 100")]
        [RegularExpression(@"[0-9]*$", ErrorMessage = "Total marks must be a number.")]
        public int MaximumMarks { get; set; }

        public virtual Course Course { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}