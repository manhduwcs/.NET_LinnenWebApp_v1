using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LinnenWebApp_v1.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(150, ErrorMessage = "Username must not exceed 150 characters.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Username cannot contain whitespace.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(150, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 150 characters.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Password cannot contain whitespace.")]
        public string Password { get; set; }
        public string? Description { get; set; }

        public int? EmployeeID { get; set; }

        public User()
        {
            // Default values to prevent null pointer
            UserID = -1;
            Username = "";
            Password = "";
            Description = "";
            EmployeeID = -1;
        }
        
        
    }
}