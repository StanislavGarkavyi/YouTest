using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace diploma.Models
{
    public class Course
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string CourseImg { get; set; }
        public string CretorId { get; set; }
        public double Price { get; set; }
        public virtual Cathegory Cathegory { get; set; }
        public virtual List<UserCourses> UserCourses { get; set; }
        public Course()
        { Lessons = new List<Lesson>();
            UserCourses = new List<UserCourses>();
        }
        public virtual List<Lesson> Lessons { get; set; }
    }
}