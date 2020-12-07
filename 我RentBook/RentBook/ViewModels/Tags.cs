using RentBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.ViewModels
{
    public class Tags
    {
        public List<Books> Books { get; set; }
        public List<BooksTags> booktag { get; set; }
        public List<int> count{ get; set; }        
        public List<int?> tid { get; set; }
        public List<string> tname { get; set; }
        public List<BooksMessage> BooksMessages { get; set; }
        public List<CmessageSqlView> CmessageSqlViews { get; set; }
        
    }
}