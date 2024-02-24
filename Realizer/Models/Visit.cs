using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realizer.Models
{
    public class Visit
    {
        [PrimaryKey,AutoIncrement]
        public int visit_id { get; set; }
        public int client_id { get; set; }
        public string date { get; set; }
        public string note { get;  set; } 
        public int total { get; set; }
        //public Visit()
        //{
        //    foreach(var purchase in Purchases)
        //    {
        //        Total += purchase.Total;
        //    }
        //}
    }
}
