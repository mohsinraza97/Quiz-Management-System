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
using System.Web.Security;
using System.Net.Mail;
using System.Data.Entity.Validation;

namespace Quizilla.Controllers
{
    public class AccountController : Controller
    {
        private QuizillaDbContext db = new QuizillaDbContext();

        [HttpGet]
        public ActionResult Register()
        {
            Utility.ResetQuiz();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (!db.Users.Any(u => u.UserId.Equals(user.UserId)))
                {
                    if (!db.Users.Any(u => u.Email == user.Email))
                    {
                        this.Session["UserId"] = user.UserId;
                        this.Session["Password"] = user.Password;
                        this.Session["Type"] = user.Type;
                        FormsAuthentication.SetAuthCookie(user.UserId, false);

                        // Save the default image for newly created user
                        string imageName = "Default.png";
                        string physicalPath = Server.MapPath("~/Content/Images/" + imageName);
                        user.Image = imageName;

                        // Add user
                        db.Users.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("Dashboard", "Home");
                    }
                    else
                    {
                        ViewBag.Error = "Email entered is already used by someone. Please try another.";
                        // If we got this far, something failed, redisplay form
                        return View(user);
                    }
                }
                else
                {
                    ViewBag.Error = "Username entered is already used by someone. Please try another.";
                    return View(user);
                }
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Login()
        {
            Utility.ResetQuiz();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            User myUser = db.Users.Where(u => u.UserId.Equals(model.UserId)
            && u.Password.Equals(model.Password)
            && u.Type.Equals(model.Type)).FirstOrDefault();

            if (myUser != null)
            {
                this.Session["UserId"] = model.UserId;
                this.Session["Password"] = model.Password;
                this.Session["Type"] = model.Type;
                FormsAuthentication.SetAuthCookie(model.UserId, false);

                //// Create the authentication ticket
                //int timeout = model.RememberMe ? 525600 : 2; // 525600 minutes = 1 year
                //var authTicket = new FormsAuthenticationTicket(model.UserId, model.RememberMe, timeout);
                //// Encrypt the ticket and add it to a cookie
                //HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                //cookie.Expires = DateTime.Now.AddMinutes(timeout);
                //cookie.HttpOnly = true;
                //Response.Cookies.Add(cookie);

                // Redirect to Dashboard
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                ViewBag.Result = false;
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            Utility.ResetQuiz();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(User user)
        {
            User myUser = db.Users.Where(u => u.Email == user.Email).FirstOrDefault();
            if (myUser != null)
            {
                string fromEmail = "quizilla.noreply0@gmail.com";
                string fromPassword = "noreply12345";
                string newPassword = this.GetRandomAlphanumericKey(new Random().Next(8, 8));
                string textMessage = "Hi " + myUser.Name + ",\n\nYou recently requested a new Quizilla password. Please use below password to login.\n\nNew password: " + newPassword + "\n\nYou can always change your password through your " + myUser.UserId + " -> Change password Settings.\n\n\nBest Regards,\nQuizilla Support Team";
                try
                {
                    // Email new password
                    MailMessage mailMessage = new MailMessage(fromEmail, user.Email, "Quizilla Reset Password", textMessage);
                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"); // for ex: smpt.yahoo.com
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(fromEmail, fromPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mailMessage);

                    // Update password in DB
                    myUser.Password = newPassword;
                    db.SaveChanges();
                    ViewBag.Result = true;
                }
                catch (Exception) { }
            }
            else
            {
                ViewBag.Result = false;
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Logoff()
        {
            Utility.ResetQuiz();
            FormsAuthentication.SignOut();

            // Drop all the information held in the session
            Session.Clear();
            Session.Abandon();

            // Redirect the user to the login page
            return RedirectToAction("Login");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Profile(string id)
        {
            Utility.ResetQuiz(); 
            id = User.Identity.Name;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile(User user, HttpPostedFileBase image)
        {
            ModelState["UserId"].Errors.Clear();
            ModelState["Type"].Errors.Clear();
            
            try
            {
                user.UserId = User.Identity.Name;
                user.Password = this.Session["Password"].ToString();
                user.Type = this.Session["Type"].ToString();
                if (image != null)
                {
                    string imageName = System.IO.Path.GetFileName(image.FileName);
                    string physicalPath = Server.MapPath("~/Content/Images/" + imageName);
                    image.SaveAs(physicalPath);
                    user.Image = imageName;
                }
                else if (image == null)
                {
                    string imageName = (from u in new QuizillaDbContext().Users
                                        where u.UserId == User.Identity.Name
                                        select u).FirstOrDefault().Image;
                    user.Image = imageName;
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Result = true;
            }
            catch (Exception)
            {
                var email = ModelState["Email"];
                if (email == null || (email != null && email.Errors.Any()))
                {
                    return View(user);
                }
                if (db.Users.Any(u => u.Email == user.Email))
                {
                    ViewBag.Error = "Email entered is already used by someone. Please try another.";
                    return View(user);
                }
            }
            return View(user);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            Utility.ResetQuiz();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            User myUser = db.Users.Where(u => u.UserId.Equals(User.Identity.Name)).FirstOrDefault();
            if (myUser != null)
            {
                if (!myUser.Password.Equals(model.OldPassword))
                {
                    ViewBag.Result = false;
                }
                else
                {
                    myUser.Password = model.NewPassword;
                    db.SaveChanges();
                    ViewBag.Result = true;
                }
            }
            return View(model);
        }

        //[HttpGet]
        //public JsonResult ValidateEmail(string email)
        //{
        //    bool isExist = db.Users.Where(u => u.Email.Equals(email)).FirstOrDefault() != null;
        //    return Json(!isExist, JsonRequestBehavior.AllowGet);
        //}
        
        [HttpGet]
        public JsonResult ValidateUserId(string userId)
        {
            bool isExist = db.Users.Where(u => u.UserId == userId).FirstOrDefault() != null;
            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }

        private string GetRandomAlphanumericKey(int length)
        {
            string alphaNumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", s = "";
            Random rand = new Random();
            for (var i = 0; i < length; i++)
            {
                s += alphaNumeric[rand.Next(0, alphaNumeric.Length)];
            }
            return s;
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
