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
    [Authorize]
    public class QuizController : Controller
    {
        private QuizillaDbContext db = new QuizillaDbContext();
        private static string _quizId;
        private static string _quizTitle;
        private static int _questionCount;
        public static int[] _questionArray;
        public static bool onlyOnceFlag = false;

        // GET: /Quiz/
        public ActionResult Index(string search, int? page)
        {
            var quizzes = from q in db.Quizzes
                          where (q.UserId.Equals(User.Identity.Name)
                          && (search == null || q.Title.Contains(search)
                          || q.QuizId.Contains(search)
                          || q.CourseId.ToString().Contains(search)
                          || q.Course.Title.Contains(search)))
                          select q;

            // Check for if the search results are not found
            if (search != null && quizzes.ToList().Count() == 0)
            {
                ViewBag.Error = "No quizzes found for '" + search + "'.";
            }
            const int pageSize = 5; // number item in 1 page
            int currentPage = (page ?? 1); // current page
            return View(quizzes.OrderBy(q => q.Date).ToPagedList(currentPage, pageSize));
        }

        // GET: /Quiz/Create
        public ActionResult Create()
        {
            ViewBag.Course = new SelectList(db.Courses.Where(c => c.UserId.Equals(User.Identity.Name)), "CourseId", "Title");
            return View();
        }

        // POST: /Quiz/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Quiz quiz)
        {
            ViewBag.Course = new SelectList(db.Courses.Where(c => c.UserId.Equals(User.Identity.Name)), "CourseId", "Title");
            if (!IsQuizValid(quiz))
            {
                return View(quiz);
            }
            else
            {
                quiz.UserId = User.Identity.Name;
                db.Quizzes.Add(quiz);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        // GET: /Quiz/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            ViewBag.Course = new SelectList(db.Courses.Where(c => c.UserId.Equals(User.Identity.Name)), "CourseId", "Title");
            return View(quiz);
        }

        // POST: /Quiz/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Quiz quiz)
        {
            ModelState["QuizId"].Errors.Clear();
            ViewBag.Course = new SelectList(db.Courses.Where(c => c.UserId.Equals(User.Identity.Name)), "CourseId", "Title");

            if (!IsQuizValid(quiz, isCreateMode: false))
            {
                return View(quiz);
            }
            else
            {
                quiz.UserId = User.Identity.Name;
                quiz.QuizId = Request.Url.Segments.Last();
                db.Entry(quiz).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        // GET: /Quiz/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // POST: /Quiz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Quiz quiz = db.Quizzes.Find(id);
            db.Quizzes.Remove(quiz);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Join()
        {
            //Utility.quizScore = 0;
            //QuizController.onlyOnceFlag = false;
            Utility.ResetQuiz();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join(JoinQuizViewModel quiz)
        {
            if (db.Quizzes.Any(q => q.QuizId.Equals(quiz.QuizId)))
            {
                _quizId = quiz.QuizId;
                if (db.Results.Any(r => r.QuizId == quiz.QuizId && r.UserId == User.Identity.Name))
                {
                    ViewBag.Error = "You have already attempted this quiz so you cannot join the quiz associated with the key '" + quiz.QuizId + "'.";
                    return View(quiz);
                }
                return RedirectToAction("Details", new { id = quiz.QuizId });
            }
            else
            {
                ViewBag.Error = "Invalid key. There is no quiz corresponding to this key.";
                return View(quiz);
            }
        }

        public ActionResult Details(string id)
        {
            //Utility.quizScore = 0;
            //QuizController.onlyOnceFlag = false;
            Utility.ResetQuiz();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (db.Results.Any(r => r.QuizId == id && r.UserId == User.Identity.Name))
            {
                ViewBag.AlreadyAttempted = true;
                ViewBag.Key = id;
                return RedirectToAction("Dashboard", "Home");
            }
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            _quizId = id;
            return View(quiz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(Quiz quiz)
        {
            _quizId = Request.Url.Segments.Last();
            return RedirectToAction("Quiz", new { id = _quizId });
        }

        public ActionResult Quiz(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (db.Results.Any(r => r.QuizId == id && r.UserId == User.Identity.Name))
            {
                ViewBag.AlreadyAttempted = true;
                ViewBag.Key = id;
                return RedirectToAction("Dashboard", "Home");
            }
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            _questionCount = db.Questions.Where(q => q.QuizId.Equals(_quizId)).ToList().Count;
            if (onlyOnceFlag == false)
            {
                _questionArray = new int[_questionCount];
                for (int i = 0; i < _questionArray.Length; i++)
                {
                    _questionArray[i] = i + 1;
                }
                onlyOnceFlag = true;
            }
            Utility.questionNo = this.GetRandomQuestionNumber();
            _questionArray = this.RemoveRandomQuestionNumberFromArray(Utility.questionNo);
            _quizTitle = db.Quizzes.Where(q => q.QuizId.Equals(_quizId)).FirstOrDefault().Title;
            return View(db.Questions.Where(q => q.QuizId.Equals(id) && q.Number == Utility.questionNo).FirstOrDefault());
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Quiz(Quiz quiz, FormCollection form)
        {
            if (_questionArray.Length != 0)
            {
                this.checkAnswer(form["option"].ToString());
                return RedirectToAction("Quiz", new { id = _quizId });
            }
            this.checkAnswer(form["option"].ToString());

            // Inserting the result into the result table
            Result result = new Result();
            result.UserId = User.Identity.Name;
            result.QuizId = _quizId;
            result.ObtainedMarks = Utility.quizScore;
            db.Results.Add(result);
            db.SaveChanges();

            Utility.ResetQuiz();
            return RedirectToAction("Score");
        }

        public ActionResult Score()
        {
            ViewBag.QuizTitle = _quizTitle;
            try
            {
                int studentScore = db.Results.Where(r => r.UserId == User.Identity.Name && r.QuizId == _quizId).FirstOrDefault().ObtainedMarks;
                if (studentScore >= db.Quizzes.Where(q => q.QuizId.Equals(_quizId)).FirstOrDefault().PassingMarks)
                {
                    ViewBag.Passed = true;
                    ViewBag.ScoreMsg = "Congratulations! You have passed the quiz. Your score is " + studentScore;
                }
                else
                {
                    ViewBag.Passed = false;
                    ViewBag.ScoreMsg = "Sorry! You have failed the quiz. Your score is " + studentScore;
                }
            }
            catch (Exception)
            {

            }
            QuizController._quizId = "";
            QuizController._quizTitle = "";
            return View();
        }

        private int GetRandomQuestionNumber()
        {
            Random rand = new Random();
            return _questionArray[rand.Next(0, _questionArray.Length)];
        }

        private int[] RemoveRandomQuestionNumberFromArray(int quesNo)
        {
            int indexToRemove = Array.IndexOf(_questionArray, quesNo);
            int[] arr = _questionArray.Where((source, index) => index != indexToRemove).ToArray();
            return arr;
        }

        private void checkAnswer(string studentAnswer)
        {
            string correctAnswer = db.Questions.Where(q => q.QuizId.Equals(_quizId) && q.Number == Utility.questionNo).FirstOrDefault().Answer;
            if (studentAnswer.Equals(correctAnswer))
            {
                ++Utility.quizScore;
            }
            System.Diagnostics.Debug.WriteLine("Question no: " + Utility.questionNo + ", Score: " + Utility.quizScore);
        }

        private bool IsQuizValid(Quiz quiz, bool isCreateMode = true)
        {
            bool check = true;
            string today = DateTime.Now.Date.ToString();
            if (isCreateMode)
            {
                if (DateTime.Parse(quiz.Date.ToString()) < DateTime.Parse(today))
                {
                    check = false;
                    ViewBag.Error = "Date of quiz must be of today or greater.";
                }
            }
            else if (quiz.PassingMarks > quiz.MaximumMarks)
            {
                ViewBag.Error = "Passing marks must be less than total marks.";
                check = false;
            }
            else if (quiz.MaximumMarks == quiz.PassingMarks)
            {
                ViewBag.Error = "Passing marks & total marks cannot be same.";
                check = false;
            }
            return check;
        }

        [HttpGet]
        public JsonResult ValidateQuizId(string quizId)
        {
            bool isExist = db.Quizzes.Where(q => q.QuizId == quizId).FirstOrDefault() != null;
            return Json(!isExist, JsonRequestBehavior.AllowGet);
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
