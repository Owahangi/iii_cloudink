using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentBook.Models
{
    public class ReadBooksModel
    {
        // Books 資料表

        public string b_id { get; set; }
        public string b_Name { get; set; }
        
        // BooksChapters 資料表
        public int bc_Chapters { get; set; }

        // BooksFiles 資料表
        public string bf_FileName { get; set; }
        public List<string> 小說書籍內容 { get; set; }

        public List<string> FilesName { get; set; }

        public string FilePath { get; set; }

        // 不在資料表內的
        public int 傳回書籍最大章節數 { get; set; }
        public string 傳回書籍章節標題 { get; set; }

    }
}