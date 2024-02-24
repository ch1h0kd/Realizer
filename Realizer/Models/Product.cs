using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realizer.Models
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int price { get; set; }
        public string category { get; set; }
        public int tax { get; set; }
        public int in_hand { get; set; }
        public int at_clients { get; set; }
        public int total {  get; set; }
        public int sold { get; set; }
        public Product Clone() => MemberwiseClone() as Product;

        public (bool IsValid, string? ErrorMessage) Validate()
        {
            if (string.IsNullOrWhiteSpace(product_name) || price == 0)
            {
                return (false, "Missing required info");
            }
            else if (price < 0 || tax < 0)
            {
                return (false, "Values cannot be negative");
            }
            else if(in_hand + at_clients != total)
            {
                return (false, "Wrong total");
            }
            return (true, null);
        }
    }
}
