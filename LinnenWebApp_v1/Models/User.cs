using System;

namespace LinnenWebApp_v1.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public int EmployeeID { get; set; }

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