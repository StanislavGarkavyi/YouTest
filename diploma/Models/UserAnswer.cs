using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace diploma.Models
{
    public class UserAnswer
    {
        [Required]
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Answer Answer { get; set; }
        public bool UsersAnswer { get; set; }
        public DateTime Date { get; set; }

    }
}