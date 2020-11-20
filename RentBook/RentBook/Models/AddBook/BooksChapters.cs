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

        // 找出目前書籍的章節 +1 (用來新增新章節時使用)
        public int CreateChapters { get; set; }
        // 用來接收章節的檔案 (多筆)
        public HttpPostedFileBase[] Files { get; set; }

        public List<string> FilesName { get; set; }
    }
}