using diploma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace diploma.Controllers
{
    public class TestController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Test
        public ActionResult Index(bool desc=true)
        {
            string query = "SELECT Courses.Name,Courses.Id, AspNetUsers.UserName as creator,sum(Courses.Price) as sum FROM[YouTest].[dbo].[UserCourses] left join Courses on Courses.Id =[UserCourses].Course_Id left join AspNetUsers on AspNetUsers.id = Courses.CretorId where[UserCourses].Confirmed = '1' group by AspNetUsers.UserName, Courses.Name ,Courses.Id order by sum ";
            query = desc ? query + " desc" : query;
            var model = db.Database.SqlQuery<TestViewModel>(query).ToList();
            return View(model);
        }
    }
    public class TestViewModel 
    {
        public string Creator { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public double Sum { get; set; }
    }
}