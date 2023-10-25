using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Records.Bases;

namespace Business.Models
{
    public class CategoryModel : RecordBase
    {
        // entity
        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long!")]
        [MaxLength(25, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long!")]
        [MaxLength(25, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Description")]
        public string Description { get; set; }

        // page-specific
        [DisplayName("Product Count")]
        public int ProductCountDisplay { get; set; }
    }
}
