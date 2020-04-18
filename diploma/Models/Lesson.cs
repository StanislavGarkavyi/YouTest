using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace diploma.Models
{
    public class Lesson
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Course Course { get; set; }
        public string File { get; set; }
        public virtual List<Question> Question { get; set; }
        public Lesson() { Question = new List<Question>(); }
    }
}