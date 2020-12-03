using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models.EditBook
{
    public class EditBookModel
    {
        // 下拉式選單使用
        public List<string> 出版社編號加名稱列表 { get; set; }
        public List<string> 作者編號加名稱列表 { get; set; }



        // Books 資料表        
        public string b_id { get; set; }
        public string b_Name { get; set; }
        public string b_Info { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string b_Image { get; set; }
        public string b_Type { get; set; }
        public DateTime b_PublishedDate { get; set; }
        public int b_DatePrice { get; set; }
        public string b_ISBN { get; set; }
        public string b_AgeRating { get; set; }
        public string b_Series_yn { get; set; }
        public string b_Put_yn { get; set; }
        public string p_id { get; set; }
        public string PublishedIdName { get; set; }

        // 其他不在資料表內的欄位
        public string 出版社編號名稱 { get; set; }
        public string b_ImagePath { get; set; }
        public string Tags字串 { get; set; }
        public List<string> 列出本書的作者 { get; set; }


        // BookAuthor 資料表
        public string a_id { get; set; }
        public string a_Name { get; set; }
        public string[] AuthorIdName { get; set; }



        // BookChapters 資料表
        public int bc_id { get; set; }
        public int bc_Chapters { get; set; }
        public string bc_Content { get; set; }
        //要新增的章節
        public int CreateChapters { get; set; }



        // BooksFiles 資料表
        // 用來接收章節的檔案 (多筆)
        public HttpPostedFileBase[] Files { get; set; }

        public List<string> FilesName { get; set; }



        // BooksTags 資料表
        public List<string> Tags { get; set; }



        // Tags 資料表
        // 外來鍵
        public int t_id { get; set; }

        // BooksTags 資料表
        public int t_Name { get; set; }
    }
}