using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace RentBook.Models.Userpage
{
    public class commentFac
    {
        public List<commentClass> getMyComment(string userEmail)
        {
            string sql = @"
select b.b_id,b.b_Type,b.b_Image,b.b_Name,bm_Score,bm.bm_Message,bm_MessageTime
from BooksMessage as bm
inner join Books as b on bm.b_id=b.b_id
where m_id=(select m_id from Member where m_Email=@userEmail)
order by bm_MessageTime desc;
";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("userEmail", userEmail));
            return getBySql(sql, paras);
        }
        private List<commentClass> getBySql(string sql, List<SqlParameter> paras)
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
            List<commentClass> list = new List<commentClass>();
            while (reader.Read())
            {
                commentClass x = new commentClass();
                x.b_id = reader["b_id"].ToString();
                x.b_Type = reader["b_Type"].ToString();
                x.b_Image = reader["b_Image"].ToString();
                x.b_Name = reader["b_Name"].ToString();
                x.bm_Score = (int)reader["bm_Score"];
                x.bm_Message = reader["bm_Message"].ToString();
                x.bm_MessageTime = (DateTime)reader["bm_MessageTime"];
                list.Add(x);
            }
            con.Close();
            return list;
        }

    }
}