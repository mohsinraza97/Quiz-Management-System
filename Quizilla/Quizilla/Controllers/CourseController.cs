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

namespace Quizilla.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private QuizillaDbContext db = new QuizillaDbContext();

        // GET: /Course/
        public ActionResult Index(string search)
        {
            var courses = from c in db.Courses
                          where (c.UserId.Equals(User.Identity.Name) 
                          && (search == null || c.Title.Contains(search)
                          || c.CourseId.ToString().Contains(search)
                          || c.Code.Contains(search)
                          || c.Credits.ToString().Contains(search)))
                          select c;
    
            // Check for if the search results are not found
            if (search != null && courses.ToList().Count() == 0)
            {
                ViewBag.Error = "No courses found for '" + search + "'.";
            }
            return View(courses.ToList());
        }

        // GET: /Course/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {
            course.UserId = User.Identity.Name;
            db.Courses.Add(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Course/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: /Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course)
        {
            course.UserId = User.Identity.Name;
            //course.CourseId = Convert.ToInt32(Request.Url.Segments.Last());
            db.Entry(course).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Course/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: /Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public JsonResult ValidateCode(string code)
        //{
        //    bool isExist = db.Courses.Where(c => c.Code == code).FirstOrDefault() != null;
        //    return Json(!isExist, JsonRequestBehavior.AllowGet);
        //}

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
