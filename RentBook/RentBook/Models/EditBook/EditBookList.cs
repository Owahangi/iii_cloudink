using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models.EditBook
{
    public class EditBookList
    {
        // Books 資料表        
        public string b_id { get; set; }
        public string b_Name { get; set; }
        public string b_Info { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string b_Image { get; set; }
        public string b_ImagePath { get; set; }
        public string b_Type { get; set; }
        public int b_DatePrice { get; set; }
        public string b_AgeRating { get; set; }
        public string 出版社編號名稱 { get; set; }
        public string b_Series_yn { get; set; }

    }
}