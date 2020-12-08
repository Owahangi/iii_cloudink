using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models
{
    public class TAGsearch
    {

        //public string bid { get; set; }
        //public string bname { get; set; }
        //public string binfo { get; set; }
        //public string btype { get; set; }
        //public string bimage { get; set; }
        public string bid { get; set; }
        public string bname { get; set; }
        public string binfo { get; set; }
        public string btype { get; set; }
        public string bimage { get; set; }

    }
    public class 書本標籤
    {
        public List<TAGsearch> tAGsearches = new List<TAGsearch>();
    }
}