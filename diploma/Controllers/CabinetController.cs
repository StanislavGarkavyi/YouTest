using diploma.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace diploma.Controllers
{
    [Authorize]
    public class CabinetController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Cabinet
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            return View(user);
        }
        public ActionResult MyCourses()
        {
            var user = User.Identity.GetUserId();
            var model = db.Courses.Where(p => p.CretorId == user).ToList();
            return PartialView(model);
        }
        public ActionResult CoursesActive(bool active)
        {
            List<UserCourses> model = new List<UserCourses>();
            var user = db.Users.Find(User.Identity.GetUserId());
            if (active)
            {  model = db.UserCourses.Where(p => p.User.Id == user.Id& p.CourseEnd==false).ToList(); }
            else
            {  model = db.UserCourses.Where(p => p.User.Id == user.Id & p.CourseEnd == true).ToList(); }
            
            return PartialView(model);
        }
        public ActionResult TestReport1(int CourseId, string RedirectUser)
        {
            string user;
            if (RedirectUser == "" || RedirectUser == null)
            { user = User.Identity.GetUserId(); }
            else
            {
                user = db.Users.Where(p => p.Email == RedirectUser).FirstOrDefault().Id;
            }
            Course course = db.Courses.Where(p => p.Id == CourseId).FirstOrDefault();
            List<TestReport1> testReports = new List<TestReport1>();
            var model = db.UserAnswers.Where(p => p.Answer.Question.Lesson.Course.Id == CourseId&&p.User.Id==user).ToList().Select(p => new { Id = p.Answer.Question.Lesson.Id, Name=p.Answer.Question.Lesson.Name}).Distinct().ToList();
            foreach (var item in model)
            {
                TestReport1 ob = new TestReport1() { Id = item.Id, Name = item.Name };
                testReports.Add(ob);
            }
            ViewBag.img =course.CourseImg;
            ViewBag.name = course.Name;
            ViewBag.desc = course.Description;
            return View(testReports);
        }
        public ActionResult TestReport2(int LessonId)
        {
            var user = User.Identity.GetUserId();
            var model = db.UserAnswers.Where(p => p.User.Id == user & p.Answer.Question.Lesson.Id == LessonId).GroupBy(p => p.Date).ToList();            
            return PartialView(model);
        }
        public ActionResult TestReport3(int LessonId, DateTime Date)
        {
            List<lessonsReport> model = new List<lessonsReport>();
            var user = User.Identity.GetUserId();
            var questions = db.Questions.Where(p => p.Lesson.Id == LessonId).ToList();
            for (int i = 0; i < questions.Count; i++)
            {
                List<AnswersReport> ListAR = new List<AnswersReport>();
                for (int j = 0; j < questions[i].Answers.Count;j++)
                {
                    AnswersReport ar = new AnswersReport();
                    ar.answer = questions[i].Answers[j].Correct;
                    ar.answerText = questions[i].Answers[j].Name;
                    var answerid = questions[i].Answers[j].Id;
                    ar.userAnswer = db.UserAnswers.Where(p => p.Date == Date & p.User.Id == user & p.Answer.Id == answerid).FirstOrDefault().UsersAnswer;
                    ListAR.Add(ar);
                }
                lessonsReport lessonsReport = new lessonsReport() { QuestionText = questions[i].Name, answersReports = ListAR };
                model.Add(lessonsReport);
                
            }
            ViewBag.Mark = getMark(Date);
            return PartialView(model);
        }
        //public ActionResult Diagram(int CourseId=61)
        //{
        //    string lessonNames="";
        //    string marks="";
        //    List<DiagramViewModel> diagramModel = new List<DiagramViewModel>();
        //    var user = User.Identity.GetUserId();
        //    var userAnswers = db.UserAnswers.Where(p => p.User.Id == user).Select(p=>new { p.Date,p.Answer.Question.Lesson.Name } ).Distinct().GroupBy(p=>p.Name).ToList();
        //    foreach (var lesson in userAnswers)
        //    {
        //        int maxMark = 0;
        //        List<int> markList = new List<int>();
        //        foreach (var item in lesson)
        //        {
        //            markList.Add(getMark(item.Date));
        //        }
        //        maxMark = markList.Max();

        //        DiagramViewModel mark = new DiagramViewModel() { Mark = maxMark, Lesson = lesson.Key };
        //        lessonNames = lessonNames + "{\"label\": \""+lesson.Key+"\"}";
        //        marks=marks+ "{\"value\": \""+maxMark+"\"}";
        //        diagramModel.Add(mark);
        //    }
        //    ViewBag.label = lessonNames;
        //    ViewBag.val = marks;
        //    return PartialView(diagramModel);
        //}
        public int getMark(DateTime date)
        {
            var user = User.Identity.GetUserId();
            var userAnswers = db.UserAnswers.Where(p => p.Date == date & p.User.Id == user).GroupBy(p => p.Answer.Question.Id).ToList();
            var questionCount = userAnswers.Count;
            var RightQuestionCount = questionCount;
            foreach (var question in userAnswers)
            {
                foreach (var answer in question)
                {
                    if (answer.UsersAnswer != answer.Answer.Correct)
                    {
                        RightQuestionCount--;
                        break;
                    }
                }
            }
            double Persent = 100 * RightQuestionCount / questionCount;
            if (Persent < 50.1) { return 2; }
            if (Persent < 70.1) { return 3; }
            if (Persent < 90.1) { return 4; } else { return 5; }
            

        }
    }
}