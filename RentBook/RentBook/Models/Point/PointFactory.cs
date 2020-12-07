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

        public bool 檢查是否有這個會員編號(string m_id)
        {
            bool check = false;

            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select * from Member where m_id=@mid";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("mid", m_id);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                check = true;
            }

            return check;
        }

        public void 儲存儲值資料(PointModel p)
        {
            bool check = 檢查是否有這個會員編號(p.m_id);

            if (check)
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
        }

        public int 判斷使用者購買書籍天數(PointModel p)
        {
            int 使用者購買的日期 = p.msd_CostPoint / p.b_DatePrice;

            return 使用者購買的日期;
        }

        // 找出目前此本書籍的剩餘時間
        public DateTime 找出目前此本書籍的剩餘時間(PointModel p)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            int 會員書櫃編號 = 回傳目前此會員的書櫃編號(p.m_id);

            string tSQL = "select bcb_BookLastTime from BookCaseBooks where bc_id=@bcid and b_id=@bid";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("@bid",p.b_id);
            cmd.Parameters.AddWithValue("@bcid", 會員書櫃編號);
            SqlDataReader reader = cmd.ExecuteReader();

            DateTime 目前此書籍的剩餘時間 = new DateTime(1990,01,01);

            if (reader.Read())
            {
               目前此書籍的剩餘時間 = (DateTime)reader["bcb_BookLastTime"];
            }

            reader.Close();
            con.Close();

            return 目前此書籍的剩餘時間;
        }

        public void 儲存消費資料(PointModel p)
        {
            bool check = 檢查是否有這個會員編號(p.m_id);

            if (check)
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

                DateTime 書籍剩餘時間 = 找出目前此本書籍的剩餘時間(p);
                DateTime 日期比較 = new DateTime(1990, 01, 01);
                int 判斷日期 = DateTime.Compare(書籍剩餘時間, 日期比較);

                string tSQL2 = "";

                if(判斷日期 == 0)
                {
                    // 將購買的書籍 加到 BookCaseBooks 資料表
                    tSQL2 = "Insert into BookCaseBooks (bc_id,b_id,bcb_BookLastTime)Values(@bcid,@bid,@bcbBookLastTime)";

                    DateTime dt = DateTime.Now;
                    DateTime 增加天數後的時間 = dt.AddDays(p.購買天數);
                    p.bcb_BookLastTime = 增加天數後的時間;
                }
                else
                {
                    // 目前書櫃已有此書籍 (所以增加購買天數)

                    DateTime dt = Convert.ToDateTime(書籍剩餘時間);
                    DateTime 增加天數後的時間 = dt.AddDays(p.購買天數);
                    p.bcb_BookLastTime = 增加天數後的時間;

                    tSQL2 = "update BookCaseBooks set bcb_BookLastTime=@bcbBookLastTime where bc_id=@bcid and b_id=@bid";
                    
                }

                SqlCommand cmd2 = new SqlCommand(tSQL2, con);
                cmd2.Parameters.AddWithValue("bcid", 會員書櫃編號);
                cmd2.Parameters.AddWithValue("bid", p.b_id);
                cmd2.Parameters.AddWithValue("bcbBookLastTime", p.bcb_BookLastTime);

                cmd2.ExecuteNonQuery();
                con.Close();
            }            
        }

        public List<PointModel> 列出月卡方案(string m_id)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();
            string tSQL = "select * from MonthlyCard";
            SqlCommand cmd = new SqlCommand(tSQL,con);
            SqlDataReader reader = cmd.ExecuteReader();

            List<PointModel> list = new List<PointModel>();

            while (reader.Read())
            {
                PointModel p = new PointModel();
                p.m_id = m_id;
                p.bmc_id = (string)reader["bmc_id"];
                p.bmc_Name = (string)reader["bmc_Name"];
                p.bmc_Date = (int)reader["bmc_Date"];
                p.bmc_Price = (int)reader["bmc_Price"];

                list.Add(p);
            }

            return list;
        }

        public void 儲存月卡資料(PointModel p)
        {
            bool check = 檢查是否有這個會員編號(p.m_id);

            if (check)
            {

                SqlConnection con = new SqlConnection(myDBConnectionString);
                con.Open();

                int 消費完的會員點數 = 回傳目前此會員的剩餘點數(p.m_id) - p.bmc_Price;
                int 會員書櫃編號 = 回傳目前此會員的書櫃編號(p.m_id);

                // 找出目前這個會員的月卡剩餘時間
                string SQL = "select isnull(m_MonthlyLastTime,GETDATE()) as m_MonthlyLastTime from Member where m_id=@mid";
                SqlCommand cmdd = new SqlCommand(SQL, con);
                cmdd.Parameters.AddWithValue("mid", p.m_id);
                SqlDataReader read = cmdd.ExecuteReader();

                if (read.Read())
                {
                    p.m_MonthlyLastTime = (DateTime)read["m_MonthlyLastTime"];
                }

                read.Close();


                // 將購買的月卡時間 增加到 Member 資料表
                DateTime 購買天數 = DateTime.Now.Date;

                // 購買三十天月卡
                if (p.bmc_Date == 30)
                {
                    DateTime dt = p.m_MonthlyLastTime;
                    購買天數 = dt.AddDays(30);
                }

                // 購買六十天月卡
                if (p.bmc_Date == 60)
                {
                    DateTime dt = p.m_MonthlyLastTime;
                    購買天數 = dt.AddDays(60);
                }

                // 購買九十天月卡
                if (p.bmc_Date == 90)
                {
                    DateTime dt = p.m_MonthlyLastTime;
                    購買天數 = dt.AddDays(90);
                }


                // 更新到會員資料表
                string tSQL = "Update Member set m_Point=@mPoint";
                if (購買天數 != DateTime.Now.Date)
                    tSQL += ",m_MonthlyLastTime=@mMonthlyLastTime";
                tSQL += " where m_id=@mid";

                SqlCommand cmd = new SqlCommand(tSQL, con);
                cmd.Parameters.AddWithValue("mid", p.m_id);
                cmd.Parameters.AddWithValue("mPoint", 消費完的會員點數);
                if (購買天數 != DateTime.Now.Date)
                    cmd.Parameters.AddWithValue("mMonthlyLastTime", 購買天數);

                cmd.ExecuteNonQuery();

                // 將消費紀錄 儲存到 MemberShopDetail 資料表
                string tSQL1 = "Insert into MemberShopDetail (m_id,b_id,msd_CostPoint,msd_DateTime,msd_TotalPoint)Values(@mid,@bid,@msdCostPoint,@msdDateTime,@msdTotalPoint)";
                SqlCommand cmd1 = new SqlCommand(tSQL1, con);
                cmd1.Parameters.AddWithValue("mid", p.m_id);
                cmd1.Parameters.AddWithValue("bid", p.bmc_id);
                cmd1.Parameters.AddWithValue("msdCostPoint", p.bmc_Price);
                cmd1.Parameters.AddWithValue("msdDateTime", DateTime.Now);
                cmd1.Parameters.AddWithValue("msdTotalPoint", 消費完的會員點數);

                cmd1.ExecuteNonQuery();

                // 找出這個會員的書櫃內的所有書籍時間
                string tSQL2 = "select * from BookCaseBooks where bc_id=@bcid";
                SqlCommand cmd2 = new SqlCommand(tSQL2, con);
                cmd2.Parameters.AddWithValue("bcid", 會員書櫃編號);
                SqlDataReader reader = cmd2.ExecuteReader();

                List<PointBookCaseBooksModel> list = new List<PointBookCaseBooksModel>();
                while (reader.Read())
                {
                    PointBookCaseBooksModel pbcb = new PointBookCaseBooksModel();
                    pbcb.bcb_id = (int)reader["bcb_id"];
                    pbcb.b_id = (string)reader["b_id"];
                    pbcb.bc_id = (int)reader["bc_id"];
                    pbcb.bcb_BookLastTime = (DateTime)reader["bcb_BookLastTime"];
                    list.Add(pbcb);
                }

                reader.Close();

                // 更新 BookCaseBooks 的書籍時間

                if (list != null)
                {
                    string tSQL3 = "";
                    SqlCommand cmd3 = new SqlCommand();
                    cmd3.Connection = con;

                    DateTime 所有書櫃裡的書增加天數 = DateTime.Now.Date;

                    foreach (PointBookCaseBooksModel pcb in list)
                    {
                        DateTime dtt = pcb.bcb_BookLastTime;
                        所有書櫃裡的書增加天數 = dtt.AddDays(p.bmc_Date);
                        string 日期格式轉換 = 所有書櫃裡的書增加天數.ToString("yyyy-MM-dd HH:mm:ss");

                        if (所有書櫃裡的書增加天數 != DateTime.Now.Date)
                        {
                            tSQL3 = "Update BookCaseBooks set bcb_BookLastTime='" + 日期格式轉換 + "' where bcb_id='" + pcb.bcb_id + "'";
                            cmd3.CommandText = tSQL3;
                            cmd3.ExecuteNonQuery();
                        }
                    }
                }

                con.Close();
            }
        }
    }
}