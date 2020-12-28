using diploma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace diploma.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            var model = new AdminViewModel();
            model.UserCourses = db.UserCourses.Where(p => !p.Confirmed).ToList();
            model.Courses = db.Courses.Where(p => !p.Confirmed).ToList();
            
            return View(model);
        }
        public ActionResult ConfirmCourse(int Course) 
        {
            db.Courses.Where(p => p.Id == Course).FirstOrDefault().Confirmed = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ConfirmUserCourse(int UserCourse) 
        {
            db.UserCourses.Where(p => p.Id == UserCourse).FirstOrDefault().Confirmed = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
    public class AdminViewModel
    {
        public List<UserCourses> UserCourses { get; set; }
        public List<Course> Courses { get; set; }
    }
}