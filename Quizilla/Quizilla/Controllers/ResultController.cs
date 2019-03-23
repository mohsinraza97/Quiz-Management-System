using Quizilla.Data;
using Quizilla.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quizilla.Controllers
{
    [Authorize]
    public class ResultController : Controller
    {
        private QuizillaDbContext db = new QuizillaDbContext();

        //
        // GET: /Result/
        public ActionResult Index(string search)
        {
            var results = from r in db.Results
                          from q in db.Quizzes
                          from c in db.Courses
                          from u in db.Users
                          where (c.CourseId == q.CourseId && r.QuizId == q.QuizId && r.UserId == u.UserId
                          && c.UserId == User.Identity.Name)
                          && (search == null || r.ResultId.ToString().Contains(search)
                          || r.User.Name.Contains(search)
                          || r.Quiz.Course.Title.Contains(search)
                          || r.Quiz.Course.Code.Contains(search)
                          || r.Quiz.Course.CourseId.ToString().Contains(search)
                          || r.QuizId.Contains(search)
                          || r.Quiz.Title.Contains(search)
                          || r.Quiz.Date.ToString().Contains(search)
                          || r.ObtainedMarks.ToString().Contains(search)
                          || r.Quiz.MaximumMarks.ToString().Contains(search))
                          select new ResultViewModel
                          {
                              ResultId = r.ResultId,
                              Student = r.User.Name,
                              Course = r.Quiz.Course.Code + ": " + r.Quiz.Course.Title,
                              Quiz = r.QuizId + ": " + r.Quiz.Title,
                              Date = r.Quiz.Date,
                              ObtainedMarks = r.ObtainedMarks,
                              MaximumMarks = r.Quiz.MaximumMarks
                          };

            // Check for if the search results are not found
            
            if (Request.IsAjaxRequest())
            {
                if (search != null && results.ToList().Count() == 0)
                {
                    ViewBag.Error = "No results found for '" + search + "'.";
                }
                return PartialView("ResultsPartial", results.ToList());
            }
            return View(results.ToList());
        }

        public ActionResult MyQuizzes(string search)
        {
            var myQuizzes = from r in db.Results
                            from q in db.Quizzes
                            from c in db.Courses
                            from u in db.Users
                            where (c.CourseId == q.CourseId && r.QuizId == q.QuizId && r.UserId == u.UserId
                            && r.UserId == User.Identity.Name)
                            && (search == null || r.Quiz.Course.Title.Contains(search)
                            || r.Quiz.Course.Code.Contains(search)
                            || r.Quiz.Course.CourseId.ToString().Contains(search)
                            || r.QuizId.Contains(search)
                            || r.Quiz.Title.Contains(search)
                            || r.Quiz.User.Name.Contains(search)
                            || r.Quiz.Date.ToString().Contains(search)
                            || r.ObtainedMarks.ToString().Contains(search)
                            || r.Quiz.MaximumMarks.ToString().Contains(search))
                            select new ResultViewModel
                            {
                                Quiz = r.QuizId + ": " + r.Quiz.Title,
                                Course = r.Quiz.Course.Code + ": " + r.Quiz.Course.Title,
                                Student = r.Quiz.User.Name,
                                Date = r.Quiz.Date,
                                ObtainedMarks = r.ObtainedMarks,
                                MaximumMarks = r.Quiz.MaximumMarks
                            };

            // Check for if the search results are not found
            if (search != null && myQuizzes.ToList().Count() == 0)
            {
                ViewBag.Error = "No quizzes found for '" + search + "'.";
            }
            return View(myQuizzes.ToList());
        }
    }
}