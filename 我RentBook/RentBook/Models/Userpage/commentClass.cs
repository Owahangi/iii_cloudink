using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models.Userpage
{
    public class commentClass
    {//b.b_Image,b.b_Name,bm_Score,bm.bm_Message,bm_MessageTime
        public string b_Type { get; set; }
        public string b_id { get; set; }
        public string b_Image { get; set; }
        public string b_Name { get; set; }
        public int bm_Score { get; set; }
        public string bm_Message {get;set;}
        public DateTime bm_MessageTime { get; set; }
    }
}