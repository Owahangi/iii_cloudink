using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace RentBook.Models.Userpage
{
    public class myWalletFac
    {
        public int getUserBalance(string userEmail)
        {
            string sql = @"select m_Point from Member	
                            where m_Email=@m_Email";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("m_Email", userEmail));
            /****/
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            if (paras != null)
            {
                foreach (SqlParameter p in paras)
                    cmd.Parameters.Add(p);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            int balance = 0;
            while (reader.Read())
            {
                if (reader["m_point"] == null)
                {
                    balance = 0;
                }
                else
                {
                    balance += (int)reader["m_point"];
                }
            }
            con.Close();
            return balance;
        }
        public List<transactionClass> getConsumptionBySql(string userEmail)
        {
            string sql = @"select msd_id as '1',msd_CostPoint as '2',msd_DateTime as '3'  from MemberShopDetail
where m_id=(select m_id from member where m_Email=@m_Email)
order by msd_DateTime desc";//
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("m_Email", userEmail));
            /****/
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            if (paras != null)
            {
                foreach (SqlParameter p in paras)
                    cmd.Parameters.Add(p);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            List<transactionClass> list = new List<transactionClass>();
            while (reader.Read())
            {
                transactionClass x = new transactionClass();
                x.orderId = (int)reader["1"];
                x.amount = (int)reader["2"];
                x.time = (DateTime)reader["3"];
                list.Add(x);
            }
            con.Close();
            return list;
        }
        public List<transactionClass> getAddValueBySql(string userEmail)
        {
            string sql = @"select mad_id as '1',mad_AddPoint as '2',mad_AddTime as '3' from MemberAddDetail
where m_id=(select m_id from member where m_Email=@m_Email)
order by mad_AddTime desc";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("m_Email", userEmail));
            /****/
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            if (paras != null)
            {
                foreach (SqlParameter p in paras)
                    cmd.Parameters.Add(p);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            List<transactionClass> list = new List<transactionClass>();
            while (reader.Read())
            {
                transactionClass x = new transactionClass();
                x.orderId = (int)reader["1"];
                x.amount = (int)reader["2"];
                x.time = (DateTime)reader["3"];
                list.Add(x);
            }
            con.Close();
            return list;
        }
    }
}