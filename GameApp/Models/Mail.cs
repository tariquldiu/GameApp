using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 

namespace GameApp.Models
{
    public class Mail
    {
        public int MailId { get; set; }
        public string CustomerName { get; set; } 
        public string Gmail { get; set; } 
        public string Phone { get; set; }
        public string Text { get; set; } 
        public string Message { get; set; }
        public DateTime UpdatedDate { get; set; } 

    }
}