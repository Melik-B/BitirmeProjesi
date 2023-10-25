using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.Filters
{
    public class ProductFilterModel
    {
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [DisplayName("Unit Price (Start)")]
        public double? UnitPriceStart { get; set; }

        public double? UnitPriceEnd { get; set; }

        [DisplayName("Stock Quantity (Start)")]
        public int? StockQuantityStart { get; set; }

        public int? StockQuantityEnd { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDateStart { get; set; }
        public DateTime? ExpirationDateEnd { get; set; }

        [DisplayName("Category")]
        public int? CategoryId { get; set; }

        [DisplayName("Store")]
        public List<int> StoreIds { get; set; }
    }
}
