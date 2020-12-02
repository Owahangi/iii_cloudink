using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models.AddChapters
{
    public class AddBookChaptersModel
    {
        // Books 資料表
        public string b_Type { get; set; }
        public string b_Series_yn { get; set; }

        // BooksFiles 資料表
        public string bc_id { get; set; }
        public string bf_FileName { get; set; }

        // 用來接收章節的檔案 (多筆)
        public HttpPostedFileBase[] Files { get; set; }
        public List<string> FilesName { get; set; }

        // BooksChapters 資料表
        public string b_id { get; set; }
        public int bc_Chapters { get; set; }
        public string bc_Content { get; set; }
    }
}