using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models
{
    public class CmessageSqlView
    {
        public int bm_id { get; set; } //書籍留言序號
        public int b_id { get; set; } //書籍編號
        public string m_id { get; set; } //會員編號
        public string m_Email { get; set; } //會員信箱
        public string m_Image { get; set; } //會員大頭貼
        public string m_Alias { get; set; } //會員暱稱
        public string bm_Message { get; set; } //評論內容
        public DateTime bm_MessageTime { get; set; } //評論時間
        public int bm_Score { get; set; } //會員對書籍的評分
    }
}