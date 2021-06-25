using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 

namespace GameApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; } 
        public string UserName { get; set; }
        public string OldPassword { get; set; } 
        public string Password { get; set; }
        public string ConfirmPassword { get; set; } 
        public string RoleName { get; set; } 
        public string Message { get; set; }
        public bool IsActive { get; set; }
    }
}