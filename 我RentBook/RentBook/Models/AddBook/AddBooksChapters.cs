using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models
{
    public class AddBooksChapters
    {
        public int bc_id { get; set; }
        public int b_id { get; set; }
        public int bc_Chapters { get; set; }
        public string bc_Content { get; set; }

        //要新增的章節
        public int CreateChapters { get; set; }
    }
}