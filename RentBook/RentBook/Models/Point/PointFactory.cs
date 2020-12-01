using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentBook.Models.Point
{
    public class PointFactory
    {
        string myDBConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";

        public int 找出書籍一日價格(string b_id)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select b_DatePrice from Books where b_id=@bid";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bid", b_id);
            SqlDataReader reader = cmd.ExecuteReader();

            int b_DatePrice = 0;

            if (reader.Read())
            {
                b_DatePrice = (int)reader["b_DatePrice"];
            }


            reader.Close();
            con.Close();

            return b_DatePrice;
        }

        public int 回傳目前此會員的剩餘點數(string m_id)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select m_Point from Member where m_id=@mid";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("mid", m_id);
            SqlDataReader reader = cmd.ExecuteReader();

            int 會員剩餘點數 = 0;

            if (reader.Read())
            {
                會員剩餘點數 = (int)reader["m_Point"];
            }

            return 會員剩餘點數;
        }

        public int 回傳目前此會員的書櫃編號(string m_id)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select bc_id from Member where m_id=@mid";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("mid", m_id);
            SqlDataReader reader = cmd.ExecuteReader();

            int 書櫃編號 = 0;

            if (reader.Read())
            {
                if(reader["bc_id"] != null)
                書櫃編號 = (int)reader["bc_id"];
            }

            return 書櫃編號;
        }

        public void 儲存儲值資料(PointModel p)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            int 儲值完的會員點數 = 回傳目前此會員的剩餘點數(p.m_id) + p.mad_AddPoint;

            // 更新會員資料表的 m_Point 點數
            string tSQL = "Update Member set m_Point=@mPoint where m_id=@mid ";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("mid", p.m_id);
            cmd.Parameters.AddWithValue("mPoint", 儲值完的會員點數);

            cmd.ExecuteNonQuery();


            // 將儲值紀錄 儲存到 MemberAddDetail 資料表
            string tSQL1 = "Insert into MemberAddDetail (m_id,mad_AddPoint,mad_AddTime,mad_TotalPoint)Values(@mid,@madAddPoint,@madAddTime,@madTotalPoint)";
            SqlCommand cmd1 = new SqlCommand(tSQL1, con);
            cmd1.Parameters.AddWithValue("mid", p.m_id);
            cmd1.Parameters.AddWithValue("madAddPoint", p.mad_AddPoint);
            cmd1.Parameters.AddWithValue("madAddTime", DateTime.Now);
            cmd1.Parameters.AddWithValue("madTotalPoint", 儲值完的會員點數);

            cmd1.ExecuteNonQuery();

            con.Close();
        }

        public int 判斷使用者購買書籍天數(PointModel p)
        {
            int 使用者購買的日期 = p.msd_CostPoint / p.b_DatePrice;

            return 使用者購買的日期;
        }

        // 傳回 今天 + 購買天數 的日期
        public DateTime 日期時間運算(int 購買天數)
        {
            DateTime dt = DateTime.Now;
            dt.AddDays(購買天數);

            return dt;
        }

        public void 儲存消費資料(PointModel p)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            int 消費完的會員點數 = 回傳目前此會員的剩餘點數(p.m_id) - p.msd_CostPoint;

            // 更新會員資料表的 m_Point 點數
            string tSQL = "Update Member set m_Point=@mPoint where m_id=@mid ";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("mid", p.m_id);
            cmd.Parameters.AddWithValue("mPoint", 消費完的會員點數);

            cmd.ExecuteNonQuery();

            // 將消費紀錄 儲存到 MemberShopDetail 資料表
            string tSQL1 = "Insert into MemberShopDetail (m_id,b_id,msd_CostPoint,msd_DateTime,msd_TotalPoint)Values(@mid,@bid,@msdCostPoint,@msdDateTime,@msdTotalPoint)";
            SqlCommand cmd1 = new SqlCommand(tSQL1, con);
            cmd1.Parameters.AddWithValue("mid", p.m_id);
            cmd1.Parameters.AddWithValue("bid", p.b_id);
            cmd1.Parameters.AddWithValue("msdCostPoint", p.msd_CostPoint);
            cmd1.Parameters.AddWithValue("msdDateTime", DateTime.Now);
            cmd1.Parameters.AddWithValue("msdTotalPoint", 消費完的會員點數);

            cmd1.ExecuteNonQuery();

            int 會員書櫃編號 = 回傳目前此會員的書櫃編號(p.m_id);

            
            p.bcb_BookLastTime = 日期時間運算(p.購買天數);

            // 將購買的書籍 加到 BookCaseBooks 資料表
            string tSQL2 = "Insert into BookCaseBooks (bc_id,b_id,bcb_BookLastTime,bcb_ReadRange)Values(@bcid,@bid,@bcbBookLastTime,@bcbReadRange)";
            SqlCommand cmd2 = new SqlCommand(tSQL2, con);
            cmd2.Parameters.AddWithValue("bcid", 會員書櫃編號);
            cmd2.Parameters.AddWithValue("bid", p.b_id);
            cmd2.Parameters.AddWithValue("bcbBookLastTime", p.bcb_BookLastTime);
            cmd2.Parameters.AddWithValue("bcbReadRange", "");

            cmd2.ExecuteNonQuery();
            con.Close();
        }
    }
}