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
        public int client_key { get; set; }
        public string number { get; set; }

        //number cannot be null
        public (bool IsValid, string? ErrorMessage) Validate()
        {
            if (number != null) //null check
            {
                var temp = new string(number.Where(Char.IsDigit).ToArray());
                if (string.IsNullOrEmpty(temp))
                {
                    return (true, "empty");
                }
                else if (temp.Length < 4)
                {
                    return (false, "Phone number has to be at least 4 digits");
                }
                else return (true, null);
            }
            else
            {
                Console.WriteLine("number is null");
                return (true, null);
            }

        }
    }
}
