using DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class UserDetail
    {
        [Key]
        public int UserId { get; set; }

        public User User { get; set; }

        [Required, StringLength(200)]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        public Gender Gender { get; set; }
    }
}