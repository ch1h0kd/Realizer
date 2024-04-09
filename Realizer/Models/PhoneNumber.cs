using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realizer.Models
{
    public class PhoneNumber
    {
        [PrimaryKey, AutoIncrement]
        public int phoneNum_id { get; set; }
        public int client_id { get; set; }
        public int number { get; set; }
    }
}
