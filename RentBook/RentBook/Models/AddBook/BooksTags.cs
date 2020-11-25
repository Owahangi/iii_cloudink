using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models.AddBook
{
    public class BooksTags
    {
        public List<string> Tags { get; set; }

        // Tags 資料表
        public string b_id { get; set; }
        // 外來鍵
        public int t_id { get; set; }

        // BooksTags 資料表
        public int t_Name { get; set; }
    }
}