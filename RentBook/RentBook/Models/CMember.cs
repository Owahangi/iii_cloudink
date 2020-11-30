using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models
{
    public class CMember
    {
        public string m_id { get; set; }
        public string m_Name { get; set; }
        public DateTime m_Birth { get; set; }
        public string m_Gender { get; set; }
        public int m_Point { get; set; }
        public string m_Email { get; set; }
        public string m_Image { get; set; }
        public DateTime m_RegisterDate { get; set; }
        public DateTime m_LastLogin { get; set; }
        public DateTime m_LastLogon { get; set; }
        public DateTime m_OnlineTime { get; set; }
        public DateTime m_MonthlyLastTime { get; set; }
        public int bc_id { get; set; }

    }
}