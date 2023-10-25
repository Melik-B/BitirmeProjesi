using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class ProductModel : RecordBase
    {
        // Fluent Validation, additional
        #region Entity
        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(2, ErrorMessage = "{0} must be at least {1} characters long!")]
        [MaxLength(100, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Product Name")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Unit Price")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} must be between {1} and {2}!")]
        public double? UnitPrice { get; set; }

        [DisplayName("Stock Quantity")]
        [Required(ErrorMessage = "{0} is required!")]
        [Range(0, 1000000, ErrorMessage = "{0} must be between {1} and {2}!")]
        public int? StockQuantity { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "{0} is required!")]
        public int? CategoryId { get; set; }

        public byte[] Image { get; set; }
        [StringLength(5)]
        public string ImageExtension { get; set; }
        #endregion

        #region Page Requirements
        [DisplayName("Unit Price")]
        public string UnitPriceDisplay { get; set; }

        [DisplayName("Expiration Date")]
        public string ExpirationDateDisplay { get; set; }

        [DisplayName("Category")]
        public string CategoryNameDisplay { get; set; }

        [DisplayName("Stores")]
        //[Required]
        public List<int> StoreIds { get; set; }

        [DisplayName("Stores")]
        public string StoreNameDisplay { get; set; }

        public int StoreId { get; set; }

        [DisplayName("Image")]
        public string ImgSrcDisplay { get; set; }
        #endregion
    }
}