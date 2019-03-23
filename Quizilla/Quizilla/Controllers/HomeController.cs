using Quizilla.Data;
using Quizilla.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quizilla.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Utility.ResetQuiz();
            ViewBag.Message = "Quizilla is a powerful quiz management system for online quizzes, provide innovative examination process and assessment solutions to educational institutions.";
            return View();
        }

        public ActionResult About()
        {
            Utility.ResetQuiz();
            ViewBag.Message = "Quizilla is a powerful quiz management system for online quizzes, provide innovative examination process and assessment solutions to educational institutions.";
            return View();
        }

        public ActionResult Contact()
        {
            Utility.ResetQuiz();
            return View();
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            Utility.ResetQuiz();
            ViewBag.Name = (from u in new QuizillaDbContext().Users
                           where u.UserId == User.Identity.Name
                           select u).FirstOrDefault().Name;
            ViewBag.Image = (from u in new QuizillaDbContext().Users
                            where u.UserId == User.Identity.Name
                            select u).FirstOrDefault().Image;
            return View();
        }
    }
}