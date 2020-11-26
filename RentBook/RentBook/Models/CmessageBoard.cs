using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models
{
    public class CmessageBoard
    {
        public int bm_id { get; set; } //書籍留言序號
        public string b_id { get; set; } //書籍編號
        public string m_id { get; set; } //會員編號
        public string bm_Message { get; set; } //留言內容
        public DateTime bm_MessageTime { get; set; } //留言時間
        public int bm_Score { get; set; } //會員對書籍的評分
    }
}