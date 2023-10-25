using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class ShoppingCartItemGroupByModel
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [DisplayName("Total Unit Price")]
        public string TotalUnitPriceDisplay { get; set; }
        public double TotalUnitPrice { get; set; }
        [DisplayName("Product Quantity")]
        public int ProductQuantity { get; set; }
    }
}
