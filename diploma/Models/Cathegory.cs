using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace diploma.Models
{
    public class Cathegory
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Img { get; set; }
        public virtual List<Course> Courses { get; set; }
    }
}