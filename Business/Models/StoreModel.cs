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
    public class StoreModel : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long!")]
        [MaxLength(200, ErrorMessage = "{0} must be at most {1} characters long!")]
        public string Name { get; set; }

        public bool IsVirtual { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [Range(1, 5, ErrorMessage = "{0} must be between {1} and {2}!")]
        [DisplayName("Rating")]
        public byte? Rating { get; set; }

        [DisplayName("Is Virtual")]
        public string IsVirtualDisplay { get; set; }
    }
}
