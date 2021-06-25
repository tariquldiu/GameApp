using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 

namespace GameApp.Models
{
    public class PaymentGateway
    {
        public int GatewayId { get; set; }
        public string GatewayName { get; set; } 
        public string AccountType { get; set; } 
        public string AccountNumber { get; set; } 
        public string Message { get; set; } 
        public int DiscountAmount { get; set; }
        public int ChargeAmount { get; set; }


    }
}