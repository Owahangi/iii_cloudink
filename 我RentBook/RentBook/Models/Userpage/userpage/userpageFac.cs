using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace RentBook.Models
{
    public class userpageFac
    {
        public List<userpageClass> getUserInfo(string userEmail)
        {
            string sql = "select isnull(m_Image,'Default.jpg') as m_Image,m_Email,m_id,m_Name,m_Alias,m_Intro from Member where m_Email = @m_Email";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("m_Email", userEmail));
            return getBySql(sql, paras);
        }
        private List<userpageClass> getBySql(string sql, List<SqlParameter> paras)
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
            List<userpageClass> list = new List<userpageClass>();
            while (reader.Read())
            {
                userpageClass x = new userpageClass();
                x.m_Image = reader["m_Image"].ToString();
                x.m_Email = reader["m_Email"].ToString() ;
                x.m_id = reader["m_id"].ToString();
                x.m_Name = reader["m_Name"].ToString();
                x.m_Alias = reader["m_Alias"].ToString();
                x.m_Intro = reader["m_Intro"].ToString();//continue
                list.Add(x);
            }
            con.Close();
            return list;
        }
    }

}