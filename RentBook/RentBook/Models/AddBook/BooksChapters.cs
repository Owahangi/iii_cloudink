using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models
{
    public class BooksChapters
    {
        public int b_id { get; set; }

        public string c_Chapters { get; set; }
        public string c_FileName { get; set; }

        public HttpPostedFileBase[] Files { get; set; }
    }
}