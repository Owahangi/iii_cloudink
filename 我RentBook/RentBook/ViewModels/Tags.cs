using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.ViewModels
{
    public class Tags
    {
        public List<Books> Books { get; set; }
        public List<string> tags{ get; set; }        
        public List<BooksTags> BooksTags { get; set; }
    }
}