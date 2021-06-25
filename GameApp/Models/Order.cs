using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 

namespace GameApp.Models 
{
    public class Order 
    { 
        public int OrderId { get; set; }  
        public string OrderNo { get; set; } 
        public string PlayerId { get; set; } 
        public string AccountType { get; set; } 
        public string AccountName { get; set; } 
        public string AccountPassword { get; set; }  
        public string AccountSecurityCode { get; set; } 
        public DateTime UpdatedDate { get; set; }
        public string GameName { get; set; }
        public string OfferName { get; set; } 
        public string Message { get; set; }
        public string ProductIds { get; set; }
        public int GameTopupId { get; set; } 
        public int UserId { get; set; }
        public string UserName { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string OrderNote { get; set; }
        public string AccountNumber { get; set; }
        public string TransactionId { get; set; }
        public double Amount { get; set; }

    }
}