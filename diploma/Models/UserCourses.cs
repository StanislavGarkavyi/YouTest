using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace diploma.Models
{
    public class UserCourses
    {
        public int Id { get; set; }
        public virtual Course Course { get; set; }
        public virtual ApplicationUser User { get; set; }
        public bool CourseEnd { get; set; }
    }
}