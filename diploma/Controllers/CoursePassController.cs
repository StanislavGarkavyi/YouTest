using diploma.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace diploma.Controllers
{
    public class CoursePassController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: CoursePass
        public ActionResult Index( int lessonid)
        {
            var lesson = db.Lessons.Where(p => p.Id == lessonid).FirstOrDefault();
            ViewBag.next = db.Lessons.Where(p => p.Id > lessonid&p.Course.Id==lesson.Course.Id).FirstOrDefault();
            ViewBag.prev = db.Lessons.Where(p => p.Id < lessonid & p.Course.Id == lesson.Course.Id).OrderByDescending(p=>p.Id).FirstOrDefault();
            return View(lesson);
        }
        public ActionResult TestPass(int LessonId)
        {
            ViewBag.date = DateTime.UtcNow.ToLocalTime().ToString();
            var model = db.Lessons.Where(p => p.Id == LessonId).FirstOrDefault();
            return View(model);
        }
        public ActionResult Question(string QuestionCount, string LessonId, string Date)
        {
           
            ViewBag.date = Date;
            DateTime date = Convert.ToDateTime(Date);
            var userid = User.Identity.GetUserId();
            var questionnumber = Convert.ToInt32(QuestionCount);
            
            var lessonid = Convert.ToInt32(LessonId);
            var question = db.Questions.Where(p => p.Lesson.Id == lessonid).ToList()[questionnumber];
            var previuosAnswer = db.UserAnswers.Where(p => p.Date == date & p.Answer.Question.Id == question.Id & p.User.Id == userid).ToList();
            if (previuosAnswer != null)
            {
                ViewBag.answer = previuosAnswer;
            }           
                                                                        var model = db.Questions.Where(p => p.Lesson.Id == lessonid).ToList()[questionnumber];
                                                                        return PartialView(model);
            
           
        }
       
        public ActionResult SaveAnswer(int QuestionId, DateTime Date, 
                                           bool checkbox0 = false,
                                           bool checkbox1 = false,
                                           bool checkbox2 = false,
                                           bool checkbox3 = false,
                                           bool checkbox4 = false)
        {
            DateTime date = Convert.ToDateTime(Date);
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(p => p.Id == userid).FirstOrDefault();
            List<bool> check = new List<bool>();
            {
                check.Add(checkbox0);
                check.Add(checkbox1);
                check.Add(checkbox2);
                check.Add(checkbox3);
                check.Add(checkbox4);

            }
            var Question = db.Questions.Where(p => p.Id == QuestionId).FirstOrDefault();
            var previuosAnswer = db.UserAnswers.Where(p => p.Date == date & p.Answer.Question.Id == Question.Id & p.User.Id == userid).ToList();
            foreach (var item in previuosAnswer) { db.UserAnswers.Remove(item); db.SaveChanges(); }
            var AnswersCount = Question.Answers.Count;
            for (int i = 0; i < AnswersCount; i++)
            {
                UserAnswer userAnswer = new UserAnswer() { Answer=Question.Answers[i], User=user, UsersAnswer=check[i], Date=date };
                db.UserAnswers.Add(userAnswer);
                db.SaveChanges();
            }

            return PartialView();
        }
       
    }
}