using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quizilla.Models
{
    public class ResultViewModel
    {
        [Display(Name = "Result Id")]
        public long ResultId { get; set; }
        public string Student { get; set; }
        public string Course { get; set; }
        public string Quiz { get; set; }
        public DateTime Date { get; set; }

        [Display(Name = "Obtained marks")]
        public int ObtainedMarks { get; set; }

        [Display(Name = "Maximum marks")]
        public int MaximumMarks { get; set; }
    }
}