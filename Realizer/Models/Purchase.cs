using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realizer.Models
{
    public class Purchase
    {
        [PrimaryKey, AutoIncrement]
        public int purchase_id { get; set; }
        public int product_id { get; set; }
        public int visit_id { get; set; }
        public int numOfPurchase { get; set; }
        public int subtotal { get; set; }
        public int discount { get; set; }
        public int total { get; set; }

        public (bool IsValid, string? ErrorMessage) Validate()
        {
            if( numOfPurchase < 0 || subtotal < 0 || discount < 0 || total < 0)
            {
                return (false, "Invalid input");
            }
            //else if( subtotal != product_id.price * numOfPurchase)
            //{
            //    return (false, "Invalid subtotal");
            //}
            //else if (total != subtotal * (product_id.Tax / 100 + 1) - discount)
            //{
            //    return (false, "Invalid total");
            //}
            return (true, null);
        }
    }
}
