using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Enums;

namespace Business.Models
{
    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long!")]
        [MaxLength(25, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Firstname")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long!")]
        [MaxLength(25, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Lastname")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long!")]
        [MaxLength(25, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(25, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(10, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Password Confirmation")]
        [Compare("Password", ErrorMessage = "Password and password confirmation must match!")]
        public string PasswordConfirmation { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(200, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        public string Address { get; set; }

        public Gender Gender { get; set; }
    }
}
