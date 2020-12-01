using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models.Point
{
    public class PointModel
    {
        // Member 資料表
        public string m_id { get; set; }
        public int m_Point { get; set; }
        public int m_MonthlyLastTime { get; set; }


        // Books 資料表
        public string b_id { get; set; }
        public int b_DatePrice { get; set; }
        public int 書籍七日價格
        {
            get
            {
                return this.b_DatePrice * 7;
            }
        }
        public int 書籍十四日價格
        {
            get
            {
                return this.b_DatePrice * 14;
            }
        }
        public int 書籍二十一日價格
        {
            get
            {
                return this.b_DatePrice * 21;
            }
        }

        // MemberAddDetail 資料表 (儲值)
        public int mad_AddPoint { get; set; }
        public DateTime mad_AddTime { get; set; }
        public int mad_Total { get; set; }

        // MemberShopDetail 資料表 (購買)
        public int msd_CostPoint { get; set; }
        public DateTime msd_AddTime { get; set; }
        public int mad_TotalPoint { get; set; }

        // BookCaseBooks 資料表
        public int bc_id { get; set; }
        public DateTime bcb_BookLastTime { get; set; }

        // 其他應用(不是資料表的欄位)
        public int 購買天數 { get; set; }

    }
}