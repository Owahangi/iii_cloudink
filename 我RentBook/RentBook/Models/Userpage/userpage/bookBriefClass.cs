using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models.Userpage
{
    public class bookBriefClass
    {
        public string b_Type { get; set; }
        public string b_id { get; set; }
        public string b_Name { get; set; }
        public string a_Name { get; set; }
        public string b_Image { get; set; }
        public DateTime bcb_BookLastTime { get; set; }
    }
}