using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using diploma.Models;
using Microsoft.AspNet.Identity;

namespace diploma.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
           
            return View();
        }
        [AllowAnonymous]
        public ActionResult CoursesMenu(string cathegory)
        {
            ViewBag.Cathegory = cathegory;
            List<Cathegory> model;
            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.userid =(user==null)?"0":user.Id;
            if (cathegory == null)
            {
                model = db.Cathegories.ToList();
                return View(model);

            }
            else
            {
                model = db.Cathegories.Where(p => p.Name == cathegory).ToList();
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult CoursesMenu(string button, string Search, string radio, string cathegory)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.userid = user.Id;
            if (cathegory == "")
            { ViewBag.Cathegory = null; }
            else
            {
                ViewBag.Cathegory = cathegory;
            }
            if (Search == "")
            { ViewBag.Search = null; }
            else { ViewBag.Search = Search; }
            ViewBag.radio = radio;
            var model = db.Cathegories.ToList();
            return View(model);
        }

        public ActionResult CourseProfile(int CourseId)
        {
            var model = db.Courses.Where(p => p.Id == CourseId).FirstOrDefault();
            ViewBag.Creator = db.Users.Where(p => p.Id == model.CretorId).FirstOrDefault().Email;
            return View(model);
        }
        public ActionResult CourseSE(int lessonid, bool end)
        {
            var Course = db.Courses.Where(p => p.Lessons.FirstOrDefault().Id == lessonid).FirstOrDefault();
            var user = db.Users.Find(User.Identity.GetUserId());
            var test = db.UserCourses.Where(p => p.Course.Id == Course.Id & p.User.Id == user.Id).FirstOrDefault();
            if (test == null)
            {
                UserCourses c = new UserCourses() { Course = Course, User = user, CourseEnd = end };
                db.UserCourses.Add(c);
                db.SaveChanges();
            }
            else
            {test.CourseEnd = end; db.SaveChanges();}
            if (!end)
            {
                return RedirectToAction("Index", "CoursePass", new { lessonid = Course.Lessons.FirstOrDefault().Id });
            }
            else
            {
                return RedirectToAction("Index", "Cabinet");
            }
            
        }
        public ActionResult UserResultRedirect(string user, int CourseId)
        {
            return RedirectToAction("TestReport1", "Cabinet", new { CourseId = CourseId,  RedirectUser=user});
        }
        public ActionResult RootCourseInfo(int CourseId=38)
        {
            var model = db.Courses.Where(p => p.Id == CourseId).FirstOrDefault();
            ViewBag.Creator = db.Users.Where(p => p.Id == model.CretorId).FirstOrDefault().Email;
            return View(model);
        }
        public ActionResult RootList(int CourseId, bool function)//true - ListLessons, false -  listUsers
        {
            if (function)
            {
               
                var model = db.Courses.Where(p => p.Id == CourseId).FirstOrDefault().Lessons.Select(p=>p.Name).ToList();               
                ViewBag.tr1 = "Название урока";
                ViewBag.tr2 = "Редактировать тест";
                ViewBag.CID = CourseId;
                return PartialView(model);
            }
            else
            {
               var model = db.Courses.Where(p => p.Id == CourseId).FirstOrDefault().UserCourses.Where(p=>p.Course.Id==CourseId).Select(p=>p.User.Email).ToList();
                ViewBag.tr1 = "Пользователь";
                ViewBag.tr2 = "Результат";
                ViewBag.CID = CourseId;
                return PartialView(model);
            }
            
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}