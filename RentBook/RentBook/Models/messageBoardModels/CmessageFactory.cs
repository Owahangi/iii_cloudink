using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace RentBook.Models
{
    public class CmessageFactory
    {
        //bm_id 書籍留言序號
        //b_id 書籍編號
        //m_id 會員編號
        //bm_Message 留言內容
        //bm_MessageTime 留言時間
        //bm_score 會員對書籍的評分
        //m_Name dbo.Member資料表的會員暱稱

        //需帶入bc_id(書櫃編號)
        public List<BooksMessage> getOneMessage(string b_id, string m_id) 
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=DESKTOP-QC55GV4\SQLEXPRESS;Initial Catalog=RentBookdb;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from BooksMessage where b_id = @b_id and m_id = @m_id ";
            cmd.Parameters.AddWithValue("b_id", b_id);
            cmd.Parameters.AddWithValue("m_id", m_id);

            List<BooksMessage> list = new List<BooksMessage>();

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read()) 
            {
                BooksMessage x = new BooksMessage();
                x.bm_id = (int)reader["bm_id"];
                x.b_id = reader["b_id"].ToString();
                x.m_id = reader["m_id"].ToString();
                x.bm_Message = reader["bm_Message"].ToString();
                x.bm_MessageTime = (DateTime)reader["bm_MessageTime"];
                x.bm_Score = (int)reader["bm_Score"];
                list.Add(x);
            }

            if (list.Count == 0)
            {
                return null;
            }

            return list;
        }
        
        public string getEmail(int bc_id) 
        {
            string email = "";
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=DESKTOP-QC55GV4\SQLEXPRESS;Initial Catalog=RentBookdb;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select m_Email from ([Member] as m inner join [BookCase] as b on m.bc_id = b.bc_id) inner join [BookCaseBooks] as bcb on m.bc_id = bcb.bc_id where bcb.bc_id = @bc_id ";
            cmd.Parameters.AddWithValue("bcb_id", bc_id);

            SqlDataReader reader = cmd.ExecuteReader();
            
            if (reader.Read())
            {
                email = (string)reader["m_Email"];
            }
            reader.Close();
            con.Close();

            return email;
            //待測試
        }

        public int getAvgSorce(string b_id)
        {
            int avgsore = 0;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=DESKTOP-QC55GV4\SQLEXPRESS;Initial Catalog=RentBookdb;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT AVG(bm_Score) as avgsore FROM BooksMessage WHERE bm_Score > 0 AND b_id = @B_ID ";
            cmd.Parameters.AddWithValue("B_ID", b_id);
            con.Close();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                avgsore = (int)reader["avgsore"];
            }
            reader.Close();
            con.Close();

            return avgsore;
            //待測試
        }

        //get join tables
        public List<CmessageSqlView> getAllmessageSqlViews()
        {
            return getMessageSqlView("select bm.bm_id, bm.b_id, m.m_id, m.m_Email, m.m_Image, m.m_Alias, bm.bm_Message, bm.bm_MessageTime, bm.bm_Score from Member as m inner join [BooksMessage] as bm on m.m_id = bm.m_id ", null);
            //SQL語法 where ... 待修正
        }

        //get join table input sql
        public List<CmessageSqlView> getMessageSqlView(string sql, List<SqlParameter> paras)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=DESKTOP-QC55GV4\SQLEXPRESS;Initial Catalog=RentBookdb;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            if (paras != null)
            {
                foreach (SqlParameter p in paras)
                {
                    cmd.Parameters.Add(p);
                }
            }
            SqlDataReader reader = cmd.ExecuteReader();
            List<CmessageSqlView> list = new List<CmessageSqlView>();
            while (reader.Read())
            {

                CmessageSqlView x = new CmessageSqlView();

                x.bm_id = (int)reader["bm_id"];
                x.b_id = reader["b_id"].ToString();
                x.m_id = reader["m_id"].ToString();
                x.m_Email = reader["m_Email"].ToString();
                x.m_Image = reader["m_Image"].ToString();
                //SQL Sever table Column(PATH -> ../../Content......)
                x.m_Alias = reader["m_Alias"].ToString();
                x.bm_Message = reader["bm_Message"].ToString();
                x.bm_MessageTime = (DateTime)reader["bm_MessageTime"];
                x.bm_Score = (int)reader["bm_score"];
                list.Add(x);
            }
            con.Close();

            return list;
        }

        //get table(BooksMessage)
        public List<CmessageBoard> getAll_BooksMessage() 
        {
            return getBySql_BooksMessage("select * from BooksMessage ", null);
        }

        public CmessageBoard getByBmId_BooksMessage(int bm_id) 
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("BM_ID",(object)bm_id));

            List<CmessageBoard> list = getBySql_BooksMessage("select * from BooksMessage where bm_id=@BM_ID", paras);
            if (list.Count == 0) 
            {
                return null;
            }

            return list[0];
        }

        // delete Message
        public void delete_BooksMessage(CmessageBoard p) 
        {
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("M_ID",(object)p.m_id));
            executeSql_BooksMessage("delete from BooksMessage where m_id=@M_ID", list);
        }

        private List<CmessageBoard> getBySql_BooksMessage(string sql, List<SqlParameter> paras) 
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=DESKTOP-QC55GV4\SQLEXPRESS;Initial Catalog=RentBookdb;Integrated Security=True";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            if (paras != null)
            {
                foreach (SqlParameter p in paras)
                {
                    cmd.Parameters.Add(p);
                }
            }
            SqlDataReader reader = cmd.ExecuteReader();
            List<CmessageBoard> list = new List<CmessageBoard>();
            while (reader.Read()) 
            {
                CmessageBoard x = new CmessageBoard();
                x.bm_id = (int)reader["bm_id"];
                x.b_id = reader["b_id"].ToString();
                x.m_id = reader["m_id"].ToString();
                x.bm_Message = reader["bm_Message"].ToString();
                x.bm_MessageTime = (DateTime)reader["bm_MessageTime"];
                x.bm_Score = (int)reader["bm_score"];
                list.Add(x);
            }
            con.Close();

            return list;
        }
        private void executeSql_BooksMessage(string sql, List<SqlParameter> paras)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=DESKTOP-QC55GV4\SQLEXPRESS;Initial Catalog=RentBookdb;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;

            if (paras != null) 
            {
                foreach (SqlParameter p in paras) 
                {
                    cmd.Parameters.Add(p);
                }
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void create__BooksMessage(CmessageBoard p) 
        {
            string sql = "insert into BooksMessage(";
            sql += "bm_Message";
            sql += ")values(";
            sql += "@BM_MESSAGE)";

            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("BM_MESSAGE",(object)p.bm_Message));
            executeSql_BooksMessage(sql, paras);
        }
    }
}