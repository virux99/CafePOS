using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class CustomerSale
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string Total { get; set; }
        public string SaleDate { get; set; }
    }

}
