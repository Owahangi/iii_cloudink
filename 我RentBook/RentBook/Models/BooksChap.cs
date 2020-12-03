using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models
{
    public class BooksChap
    {
        public Books Books { get; set; }
        public List<BooksChapters> Chapters { get; set; }
        public List<BooksMessage> Messages { get; set; }
        public List<Member> Members { get; set; }
        public List<CmessageSqlView> CmessageSqlViews { get; set; }
        public List<BookCaseBooks> BookCaseBooks { get; set; }
        public bool log { get; set; }
        public CMember cmember { get; set; }
    }
}