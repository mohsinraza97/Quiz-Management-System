using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quizilla.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,20}$", ErrorMessage = "Invalid username. e.g. Emad01")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 20 characters.")]
        [Remote("ValidateUserId", "Account", ErrorMessage = "This username is already used by someone. Please try another.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[a-zA-Z-\s]{1,40}$", ErrorMessage = "Invalid name. e.g. Emad-ud-din")]
        [StringLength(40, MinimumLength = 1, ErrorMessage = "Name cannot be longer than 40 characters.")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^(?("")("".+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]\.)+[a-z]{2,6}))$", ErrorMessage = "Email entered is invalid. e.g. emad01@example.com")]
        [StringLength(70, MinimumLength = 1, ErrorMessage = "Email cannot be longer than 70 characters.")]
        [Index(IsUnique = true)]
        [Remote("ValidateEmail", "Account", ErrorMessage = "Invalid email. It is already used by someone. Please try another.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters.")]
        public string Password { get; set; }

        [DataType(DataType.Upload)]
        public string Image { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        [StringLength(7)]
        public string Type { get; set; } // Teacher or Student
        
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}