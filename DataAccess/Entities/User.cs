using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class User : RecordBase
    {
        [Required, MinLength(3), MaxLength(25)]
        public string Firstname { get; set; }

        [Required, MinLength(3), MaxLength(25)]
        public string Lastname { get; set; }

        [Required, MinLength(3), MaxLength(25)]
        public string Username { get; set; }

        [Required, StringLength(25)]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public UserDetail UserDetail { get; set; }
    }
}
