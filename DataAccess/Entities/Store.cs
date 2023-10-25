using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Store : RecordBase
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public bool IsVirtual { get; set; }

        public byte? Rating { get; set; }

        public List<ProductStore> ProductStores { get; set; }
    }
}
