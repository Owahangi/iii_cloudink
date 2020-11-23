using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models
{
    public class BooksFiles
    {
        public string b_id { get; set; }

        // 用來接收章節的檔案 (多筆)
        public HttpPostedFileBase[] Files { get; set; }

        public List<string> FilesName { get; set; }
    }
}