using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace RentBook.Models.Userpage
{
    public class shareFac
    {
        public string getMyPicture(string userEmail)
        {
            string sql = @"select m_Image from Member where m_Email=@userEmail;";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("userEmail", userEmail));
            return getPicBySql(sql, paras);
        }
        private string getPicBySql(string sql, List<SqlParameter> paras)
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
                    cmd.Parameters.Add(p);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            string MyPicture = "";
            while (reader.Read())
            {
                MyPicture += reader["m_Image"].ToString();
            }
            con.Close();
            return MyPicture;
        }/*
        private void executeSql(string sql, List<SqlParameter> paras)
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
                    cmd.Parameters.Add(p);
            }
            cmd.ExecuteNonQuery();
            con.Close();
        }*/
    }
}