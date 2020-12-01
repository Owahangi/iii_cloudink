using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models.Author
{
    public class AuthorModel
    {
        // Author 資料表
        public string a_id { get; set; }
        public string a_Name { get; set; }
        public string a_Image { get; set; }
        public HttpPostedFileBase image { get; set; }
        public string a_Birth { get; set; }
        public int a_Point { get; set; }
        public string a_Email { get; set; }
    }
}