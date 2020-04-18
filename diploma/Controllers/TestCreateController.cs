using diploma.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace diploma.Controllers
{
    [Authorize]
    public class TestCreateController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: TestCreate
        public ActionResult Test(string QuestionCount , int LessonId)
        {
            ViewBag.LessonId = LessonId;
            var lesson = db.Lessons.Where(p => p.Id == LessonId).FirstOrDefault();
            ViewBag.LessonName = lesson.Name;
            QuestionCount = db.Questions.Where(p => p.Lesson.Id == lesson.Id).Count().ToString();
            if (QuestionCount == null || QuestionCount == "")
            {
                ViewBag.QuestionCount = 0;
            }
            else
            {
                ViewBag.QuestionCount = Convert.ToInt32(QuestionCount);
            }
            return View();
        }
        public ActionResult Question(int QuestionCount, int LessonId)
        {
            ViewBag.LessonId = LessonId;
            ViewBag.QuestionCount = QuestionCount;
            return PartialView();
        }
        public ActionResult AddQuestion(int LessonId, int QuestionCount)
        {
            ViewBag.LessonId = LessonId;
            ViewBag.QuestionCount = QuestionCount;

            var testid = db.Lessons.Where(p => p.Id == LessonId).FirstOrDefault();
            var question = db.Questions.Where(p => p.Lesson.Id == testid.Id).ToList();


            var modelquestioncount = question.Count();
            if (modelquestioncount > QuestionCount)
            {
                ViewBag.PreviousQuestion = question[QuestionCount].Id;
                ViewBag.Question = question[QuestionCount].Name;
                var checkquestion = question[QuestionCount].Id;
                var answers = db.Answers.Where(p => p.Question.Id == checkquestion).ToList();
                ViewBag.image = question[QuestionCount].Image;
                return PartialView(answers);
            }
            else
            { return PartialView(); }
        }
        public ActionResult Result(List<string> answer, string Question, string LessonsId, 
                                    int QuestionCount, HttpPostedFileBase QuestionImg,
                                    bool Update, string PreviousQuestion, string button,
                                           bool checkbox0 = false,
                                           bool checkbox1 = false,
                                           bool checkbox2 = false,
                                           bool checkbox3 = false,
                                           bool checkbox4 = false)
        {
            if (button == "Сохранить вопрос")
            {
                try
                {
                    var previousQuestion = Convert.ToInt32(PreviousQuestion);
                    string path = "";
                    List<Answer> oldanswers = new List<Answer>();

                    Question question;
                    int id = Convert.ToInt32(LessonsId);
                    var Lessonid = db.Lessons.Where(p => p.Id == id).FirstOrDefault();
                    if (!Update)
                    {
                        db.Questions.Add(new Question { Name = Question, Lesson = Lessonid, Image = path });
                        db.SaveChanges();
                        question = db.Questions.Where(p => p.Name == Question & p.Lesson.Id == Lessonid.Id).FirstOrDefault();
                        if (QuestionImg != null)
                        {
                            question.Image = Photo(QuestionImg, "QuestionImages", question.Id.ToString());
                            db.SaveChanges();
                        }

                    }
                    else
                    {
                        ////изменяем вопрос
                        question = db.Questions.Where(p => p.Id == previousQuestion).FirstOrDefault();
                        question.Name = Question;
                        if (QuestionImg != null)
                        {
                            question.Image = Photo(QuestionImg, "QuestionImages", question.Id.ToString());
                        }
                        db.SaveChanges();
                        ////берем старые вопросы
                        oldanswers = db.Answers.Where(p => p.Question.Id == question.Id).ToList();
                        foreach (var item in oldanswers)
                        {
                            db.Answers.Remove(item);
                            db.SaveChanges();
                        }
                    }
                    List<bool> check = new List<bool>();
                    {
                        check.Add(checkbox0);
                        check.Add(checkbox1);
                        check.Add(checkbox2);
                        check.Add(checkbox3);
                        check.Add(checkbox4);

                    } ///заполнение списков 
                    for (int i = 0; i < 5; i++)
                        if (answer[i] != "")
                        {
                            db.Answers.Add(new Answer { Name = answer[i], Correct = check[i], Question = question });
                            db.SaveChanges();
                        }
                    var newanswers = db.Answers.Where(p => p.Question.Id == question.Id).ToList();
                    for (int i = 0; i < oldanswers.Count(); i++)
                    {
                        string OAId = oldanswers[i].Id.ToString();
                        var useranswers = db.UserAnswers.Where(p => p.Answer.Name == OAId).ToList();
                        if (useranswers != null)
                        {
                            foreach (var item in useranswers)
                            {
                                item.Answer.Name = newanswers[i].Id.ToString();
                                db.SaveChanges();
                            }
                        }

                    }
                    ViewBag.Suc = true;
                    ViewBag.Message = "Вопрос уcпешно добавлен!";
                    return PartialView();
                }
                catch (Exception)
                {
                    ViewBag.Suc = false;
                    ViewBag.Message = "Вопрос не добавлен! Наверное...";
                    return PartialView();
                }
            }
            else
            {
                try
                {
                    var previousQuestion = Convert.ToInt32(PreviousQuestion);
                    var question = db.Questions.Where(p => p.Id == previousQuestion).FirstOrDefault();
                    var oldanswers = db.Answers.Where(p => p.Question.Id == question.Id).ToList();
                    foreach (var item in oldanswers)
                    {
                        db.Answers.Remove(item);
                        db.SaveChanges();
                    }
                    db.Questions.Remove(question);
                    db.SaveChanges();
                    ViewBag.Suc = true;
                    ViewBag.Message = "Вопрос успешно удален";
                    return PartialView();
                }
                catch (Exception)
                {
                    ViewBag.Suc = false;
                    ViewBag.Message = "Нельзя удалить вопрос, который не внесен ";
                    return PartialView();
                }
            }
        }
        public string Photo(HttpPostedFileBase Picture, string folder, string id)
        {
            if (Picture != null)
            {
                Rectangle size;
                //Rectangle size = new Rectangle(0,0,100, 100);
                Bitmap image = new Bitmap(Picture.InputStream);
                var width = image.Width;
                var height = image.Height;
                if (width > height)
                {
                    size = new Rectangle((width - height) / 2, 0, height, height);
                }
                else
                {
                    size = new Rectangle(0, (height - width) / 2, width, width);
                }
                Bitmap cropImg = new Bitmap(image.Clone(size, image.PixelFormat));
                Bitmap finalImg = new Bitmap(cropImg, new Size(500, 500));
                string path = Path.Combine(HttpRuntime.AppDomainAppPath, String.Concat(folder, "\\", id, ".jpg"));
                finalImg.Save(path, ImageFormat.Jpeg);
                image.Dispose();
                cropImg.Dispose();
                finalImg.Dispose();
                return "/" + folder + '/' + id + ".jpg";

            }
            else return "";
        }
    }
}