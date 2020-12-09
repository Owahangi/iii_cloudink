using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models.Userpage.bookshelf
{
    public class doubleClass2
    {
        public IEnumerable<bookBriefClass> unexpiredBook { get; set; }
        public IEnumerable<bookBriefClass> expiredBook { get; set; }
    }
}