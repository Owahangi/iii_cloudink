using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models.Point
{
    // 這個 Model 用來撈出所有會員書櫃內的書籍資訊
    public class PointBookCaseBooksModel
    {
        // BookCaseBooks 資料表
        public int bc_id { get; set; }
        public string b_id { get; set; }
        public DateTime bcb_BookLastTime { get; set; }
    }
}