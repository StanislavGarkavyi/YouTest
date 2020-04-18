using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace diploma.Models
{
    public class BalanceHistory
    {
        public int id { get; set; }        
        public double Sum { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Course Course { get; set; }
    }
}