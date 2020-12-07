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
        public List<string> bid = new List<string>();
        public List<string> bname = new List<string>();
        public List<string> binfo = new List<string>();
        public List<string> btype = new List<string>();
        public List<string> bimage = new List<string>();

    }
    public class 書本標籤
    {
        public List<TAGsearch> tAGsearches = new List<TAGsearch>();
    }
}