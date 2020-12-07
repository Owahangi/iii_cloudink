using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.ViewModels
{
    public class Home_VM
    {
        public Models.CMember Member { get;set; }
        public List<Books> Books { get; set; }
    }
}