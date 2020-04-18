using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace diploma.Models
{
    public class Question
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Image { get; set; }

       

        public virtual Lesson Lesson { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public Question() { Answers = new List<Answer>(); }
    }
}