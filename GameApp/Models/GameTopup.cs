using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 

namespace GameApp.Models
{
    public class GameTopup 
    { 
        public int GameTopupId { get; set; }
        public string GameName { get; set; } 
        public string ImageUrl { get; set; } 
        public string OfferName { get; set; }
        public bool IsEnterPlayerId { get; set; } 
        public bool IsSocialAccount { get; set; } 
        public bool IsAccountType { get; set; } 
        public bool IsFacebookAccount { get; set; } 
        public bool IsGmailAccount { get; set; } 
        public bool IsAccountPassword { get; set; } 
        public bool IsAccountSecurityCode { get; set; } 
        public string PrePurchaseMessage { get; set; } 
        public string Guideline { get; set; }  
        public string ProductIds { get; set; }
        public string Message { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }


    }
}