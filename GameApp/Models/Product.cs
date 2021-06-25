using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 

namespace GameApp.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } 
        public double ProductPrice { get; set; } 
        public string ProductType { get; set; }  
        public string Currency { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Message { get; set; } 
        public bool IsActive { get; set; }


    }
}