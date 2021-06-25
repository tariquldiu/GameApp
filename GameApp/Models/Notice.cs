using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 

namespace GameApp.Models
{
    public class Notice
    {
        public int NoticeId { get; set; }
        public string Description { get; set; } 
        public int PositiveVote { get; set; } 
        public int NegativeVote { get; set; }
        public bool HasVoting { get; set; } 
        public string Message { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }


    }
}