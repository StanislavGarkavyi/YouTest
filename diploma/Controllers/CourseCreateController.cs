using diploma.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.IO.Compression;

namespace diploma.Controllers
{
    [Authorize]
    public class CourseCreateController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: CourseCreate
       
        public ActionResult Index(string CourseId)
        {
            SelectList Cathegories = new SelectList(db.Cathegories.Select(p => p.Name).ToList());
            ViewBag.Cathegories = Cathegories;
            if (CourseId == ""||CourseId==null)
            {
                return View();
            }
            else
            {
                int Id = Convert.ToInt32(CourseId);
                var CourseModel= db.Courses.Where(p => p.Id == Id).FirstOrDefault();
                ViewBag.Id = CourseModel.Id;
                ViewBag.Name = CourseModel.Name;
                ViewBag.Description = CourseModel.Description;
                ViewBag.Cathegory = CourseModel.Cathegory.Name;
                return View();
            }
            
        }
        [HttpPost]
        public ActionResult Index(CourseCreateViewModel model, string id)//id=""
        {
            if (ModelState.IsValid)
            {
                Cathegory cat = db.Cathegories.Where(p => p.Name == model.Cathegory).FirstOrDefault();
                string userid = User.Identity.GetUserId();
                TestCreateController control = new TestCreateController();
                if (id == "")
                {
                    Course c = new Course { Name = model.Name, Description = model.Description, Cathegory = cat, CretorId = userid, Confirmed=false, Price= model.Price };
                    db.Courses.Add(c);
                    db.SaveChanges();
                    var param = db.Courses.OrderByDescending(p => p.Id).FirstOrDefault();
                    if (model.img != null)
                    {
                        c.CourseImg = control.Photo(model.img, "CourseImg", param.Id.ToString());
                        db.SaveChanges();
                    }

                    return RedirectToAction("LessonBuilder", new { courseid = param.Id });
                }
                else
                {
                    int CourseID = Convert.ToInt32(id);
                    var c = db.Courses.Where(p => p.Id == CourseID).FirstOrDefault();
                    c.Name = model.Name;
                    c.Description = model.Description;
                    c.Cathegory = cat;
                    if (model.img != null)
                    {
                        System.IO.File.Delete(Server.MapPath(c.CourseImg));
                        c.CourseImg = control.Photo(model.img, "CourseImg", c.Id.ToString());
                        db.SaveChanges();
                    }
                    db.SaveChanges();

                    return RedirectToAction("LessonEditor", new { CourseId = c.Id });

                }

            }
            else {
                SelectList Cathegories = new SelectList(db.Cathegories.Select(p => p.Name).ToList());
                ViewBag.Cathegories = Cathegories;
                return View();
            }




        }
        public ActionResult LessonEditor(int CourseId)
        {
            var course = db.Courses.Where(p => p.Id == CourseId).FirstOrDefault();
            ViewBag.LessonCount = course.Lessons.Count();
            return View(course);
        }
        [HttpPost]
        public ActionResult LessonEditor(string submit, string number, List<string> lessonname, List<HttpPostedFileBase> lessonfile, List<int> lessonid,  int ID)
        {
           var Course= db.Courses.Where(p => p.Id == ID).FirstOrDefault();
            //edit old records
            for (int i = 0; i < lessonid.Count; i++)
            {
                var lid = lessonid[i];
                var lesson = db.Lessons.Where(p => p.Id == lid).FirstOrDefault();
                var path = Server.MapPath("/CoursesFiles/" + lesson.Course.Id.ToString() + "/" + lesson.Id);
                lesson.Name = lessonname[i];
                db.SaveChanges();
                if (lessonfile[i] != null)
                {
                    Delete(path);
                    Save(lesson.Course.Id, lesson.Id, lessonfile[i]);
                }
            }
            //add new records
            for(int i=lessonid.Count; i<lessonname.Count;i++)
            {
                Lesson NewLesson = new Lesson { Name=lessonname[i], Course=Course  };
                db.Lessons.Add(NewLesson);
                db.SaveChanges();
                NewLesson = db.Lessons.OrderByDescending(p => p.Id).FirstOrDefault();
                var path = Server.MapPath("/CoursesFiles/" + NewLesson.Course.Id.ToString() + "/" + NewLesson.Id + "/index.html");
                NewLesson.File = "/CoursesFiles/" + NewLesson.Course.Id.ToString() + "/" + NewLesson.Id + "/index.html";
                db.SaveChanges();
                if (lessonfile[i] != null) { Save(NewLesson.Course.Id, NewLesson.Id, lessonfile[i]); }
            }



            if (submit != "Ок")
            {
                ViewBag.save = "Ok";
            }
            
            ViewBag.LessonCount = number;
            var course = db.Courses.Where(p => p.Id == ID).FirstOrDefault();
            return View(course);

        }

        public ActionResult LessonBuilder(int courseid)
        {
            ViewBag.course = db.Courses.Where(p => p.Id == courseid).FirstOrDefault().Name.ToString();
            ViewBag.couseid = courseid;
            return View();
        }
        [HttpPost]
        public ActionResult LessonListBuilder(int number, int couseid)
        {
            ViewBag.couseid = couseid;
            ViewBag.Number = number;           
            return PartialView();
        }
        public void Save(int courseid, int lessonid, HttpPostedFileBase file)
        {
            string path = Server.MapPath("~/CoursesFiles/" + courseid + "/" + lessonid + "/");
            using (ZipArchive archive = new ZipArchive(file.InputStream))
            {
                string s = archive.Entries[0].FullName;
                System.IO.Directory.CreateDirectory(Server.MapPath("~/CoursesFiles/" + courseid + "/" + lessonid));
                for (int counter = 1; counter < archive.Entries.Count; counter++)
                {
                    string fullname = archive.Entries[counter].FullName.Remove(0, s.Length);
                    if (!string.IsNullOrEmpty(Path.GetExtension(fullname))) //make sure it's not a folder
                    {
                        archive.Entries[counter].ExtractToFile(Path.Combine(path, fullname)); //создает файлы
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.Combine(path, fullname));//создает папку
                    }
                }

            }
            var lesson = db.Lessons.Where(p => p.Id == lessonid).FirstOrDefault();
            string html = System.IO.File.ReadAllText(path+"index.html");
            int search = html.IndexOf("url(img/");
            while (search > 0)
            {
                html = html.Insert(search + 4, "../../CoursesFiles/"+courseid.ToString()+"/"+lessonid.ToString()+"/");
                search = html.IndexOf("url(img/");
            }
            search = html.IndexOf("src=\"img");
            while (search > 0)
            {
                html = html.Insert(search + 5, "../../CoursesFiles/"+courseid.ToString()+"/"+lessonid.ToString()+"/");
                search = html.IndexOf("src=\"img");
            }
            System.IO.File.WriteAllText(path + "index.html", html);

        }
        public void Delete(string path)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
        
        public ActionResult Result(List<string>lessonname, List<HttpPostedFileBase> lessonfile, int courseid)
        {
            var course = db.Courses.Where(p => p.Id == courseid).FirstOrDefault();
            System.IO.Directory.CreateDirectory(Server.MapPath("~/CoursesFiles/"+course.Id));
            List<Lesson> courselessons = new List<Lesson>();
            int i = 0;
            foreach (var item in lessonfile)
            {
                Lesson lesson = new Lesson();
                lesson.Name = lessonname[i]; i++;
                lesson.Course = course;               
                db.Lessons.Add(lesson);
                courselessons.Add(lesson);
                db.SaveChanges();
                Save(course.Id, lesson.Id, item);
                lesson.File = "~/CoursesFiles/" + course.Id + "/" + lesson.Id + "/index.html";
                db.SaveChanges();
            }
            Course c = db.Courses.Where(p => p.Id == course.Id).FirstOrDefault();
            c.Lessons = courselessons;
            db.SaveChanges();
            ViewBag.course = course.Id;            
            return PartialView();
        }
        public ActionResult TestsList(int courseid)
        {
            var model = db.Courses.Where(p => p.Id == courseid).FirstOrDefault();
            return View(model);
        }

        public ActionResult CathegoryCreate()
        { return View(); }
        [HttpPost]
        public ActionResult CathegoryCreate(HttpPostedFileBase CatImg, string category)
        {
            TestCreateController test = new TestCreateController();
            string path = "";
            db.Cathegories.Add(new Cathegory { Name = category, Img = path });
            db.SaveChanges();
            var cat = db.Cathegories.OrderByDescending(p => p.Id).FirstOrDefault();
            if (CatImg != null)
            {
                path =test.Photo(CatImg, "CathegoryImg", cat.Id.ToString());
            }
            else
            {
                path = "/CathegoryImg/default.jpg";
            }
            cat.Img = path;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}