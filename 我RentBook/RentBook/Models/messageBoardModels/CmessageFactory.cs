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

        public List<CmessageSqlView> getAllmessageSqlViews()
        {
            return getMessageSqlView("select bm.bm_id, bm.b_id, m.m_id, m.m_Email, m.m_Image, m.m_Alias, bm.bm_Message, bm.bm_MessageTime from Member as m inner join [BooksMessage] as bm on m.m_id=bm.m_id ", null);
            //SQL語法 where ... 待修正
        }

        public List<CmessageSqlView> getMessageSqlView(string sql, List<SqlParameter> paras)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";
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
                
                list.Add(x);
            }
            con.Close();


            return list;
        }
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