using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Quizilla.Models;
using Quizilla.Data;
using PagedList;

namespace Quizilla.Controllers
{
    public class QuestionController : Controller
    {
        private QuizillaDbContext db = new QuizillaDbContext();
        private static string _quizId;
        const int pageSize = 5; // number item in 1 page

        // GET: /Question/
        public ActionResult Index([Bind(Prefix = "id")] string quizId, int? page)
        {
            _quizId = quizId;
            if (quizId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var quiz = db.Quizzes.Find(quizId);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            ViewData["PageSize"] = pageSize;
            ViewData["CurrentPage"] = (page ?? 1);
            return View(quiz);
        }

        [ValidateInput(false)]
        public ActionResult Search([Bind(Prefix = "id")] string quizId, string search, int? page)
        {
            if (quizId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.QuizId = quizId;
            ViewBag.Search = search;
            var questions = from q in db.Questions
                            where (q.QuizId.Equals(quizId)
                            && (search == null || q.Description.Contains(search)
                            || q.QuestionId.ToString().Equals(search)
                            || q.Number.ToString().Equals(search)
                            || q.Type.Contains(search)
                            || q.Option1.Contains(search)
                            || q.Option2.Contains(search)
                            || q.Option3.Contains(search)
                            || q.Option4.Contains(search)
                            || q.Answer.Contains(search)))
                            select q;

            // Check for if the search results are not found
            if (search != null && questions.ToList().Count() == 0)
            {
                var quiz = from q in db.Quizzes
                           where q.QuizId.Equals(quizId)
                           select q;

                ViewBag.Error = "No questions found for '" + search + "' in " + quiz.FirstOrDefault().Title + ".";
            }
            int currentPage = (page ?? 1); // current page
            return View(questions.OrderBy(q => q.QuestionId).ToPagedList(currentPage, pageSize));
        }

        // GET: /Question/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: /Question/Create
        public ActionResult Create(string quizId)
        {
            int questionCount = (db.Questions.Where(q => q.QuizId.Equals(quizId)).ToList().Count) + 1;
            ViewBag.questionNo = questionCount;
            return View();
        }

        // POST: /Question/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question question)
        {
            if (!IsQuestionValid(question))
            {
                return View(question);
            }
            db.Questions.Add(question);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = question.QuizId });
        }

        // GET: /Question/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: /Question/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Question question)
        {
            question.QuizId = _quizId;
            if (!IsQuestionValid(question))
            {
                return View(question);
            }
            db.Entry(question).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = question.QuizId });
        }

        // GET: /Question/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: /Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Question question = db.Questions.Find(id);
            var routeValue = new { id = question.QuizId };
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index", routeValue);
        }

        private bool IsQuestionValid(Question question)
        {
            bool check = true;
            if (question.Type == "T/F")
            {
                if (!question.Option1.Equals("True"))
                {
                    ViewBag.Error = "Option 1 must be True for the T/F type question.";
                    check = false;
                }
                else if (!question.Option2.Equals("False"))
                {
                    ViewBag.Error = "Option 2 must be False for the T/F type question.";
                    check = false;
                }
                else if (!(question.Answer.Equals(question.Option1) ||
                    question.Answer.Equals(question.Option2)))
                {
                    ViewBag.Error = "Answer must be True or False for the T/F type question.";
                    check = false;
                }
            }
            else if (question.Type == "MCQS")
            {
                if (question.Option3 == null)
                {
                    ViewBag.Error = "Option 3 is required for the MCQS type question.";
                    check = false;
                }
                else if (question.Option4 == null)
                {
                    ViewBag.Error = "Option 4 is required for the MCQS type question.";
                    check = false;
                }
                else if (!(question.Answer.Equals(question.Option1) ||
                    question.Answer.Equals(question.Option2) ||
                    question.Answer.Equals(question.Option3) ||
                    question.Answer.Equals(question.Option4)))
                {
                    ViewBag.Error = "Answer must be from the options provided above for the MCQS type question.";
                    check = false;
                }
            }
            return check;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
