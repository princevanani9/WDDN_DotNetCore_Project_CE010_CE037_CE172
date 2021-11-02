using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChattingApplication.Models
{
    public class User
    {
        
        public int Id { get; set; }
        [Required,MinLength(6,ErrorMessage ="Username cannot less than 6")]
        public string Username { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",ErrorMessage ="Invalid Email Format")]
        public string Email { get; set; }
        [Required, MinLength(6, ErrorMessage = "Password cannot less than 6")]
        public string Password { get; set;  }
        [Required]
        [Compare("Password",ErrorMessage ="Confirm Password and Password should be match!")]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression("[1-9]{1}[0-9]{9}", ErrorMessage ="Mobile Number contain 10 digit only")]
        
        public string Mobile { get; set; }
        [Required]

        public string Photopath { get; set; }
    }
}
