using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realizer.Models
{
    public class Client
    {
        [PrimaryKey, AutoIncrement]
        public int client_key { get; set; } //immutable
        public int client_id { get; set; } //mutable, for user use
        public string client_name { get; set; }
        public string zip { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string detail_address { get; set; }
        public string apt_num { get; set; }
        public string nickname { get; set; }
        //[Column (TypeName = "TEXT")]
        public string date_started { get; set; }
        public string date_next { get; set; }
        public string note { get; set; }

        public Client Clone() => MemberwiseClone() as Client;

        public (bool IsValid, string? ErrorMessage) Validate()
        {
            if (string.IsNullOrWhiteSpace(client_name) || client_id == 0)
            {
                return (false, "Missing required info");
            }
            else if(client_id < 0)
            {
                return (false, "ID cannot be negative");
            }
            return (true, null);
        }
    }
}
