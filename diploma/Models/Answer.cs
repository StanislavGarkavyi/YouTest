using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace diploma.Models
{
    public class Answer
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public bool Correct { get; set; }


        public virtual Question Question { get; set; }
        public virtual List<UserAnswer> UserAnswers { get; set; }
        public Answer() { UserAnswers = new List<UserAnswer>(); }
    }
}