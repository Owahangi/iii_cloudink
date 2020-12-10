using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace RentBook.Models.Userpage
{
    public class wishlistFac
    {
        public List<wishlistClass> getWishlist(string userEmail)
        {
            string sql = @"select b.b_Type,b.b_id,b_name,a.a_Name,b.b_Info,b.b_Image,b.b_DatePrice 
from (([books] as b
inner join [BooksAuthor] as ba on b.b_id=ba.b_id)
inner join [Author] as a on a.a_id=ba.a_id)
inner join [BooksWishlist] as bw on bw.b_id=b.b_id
where bc_id=(select bc_id from Member where m_Email=@m_Email)
order by bw_AddTime desc
";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("m_Email", userEmail));
            return getBySql(sql, paras);
        }
        private List<wishlistClass> getBySql(string sql, List<SqlParameter> paras)
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
            List<wishlistClass> list = new List<wishlistClass>();
            while (reader.Read())
            {
                wishlistClass x = new wishlistClass();
                x.b_Type = reader["b_Type"].ToString();
                x.b_id = reader["b_id"].ToString();
                x.b_Name = reader["b_Name"].ToString();
                x.b_Info = reader["b_Info"].ToString();
                x.b_Image = reader["b_Image"].ToString();
                x.a_Name = reader["a_Name"].ToString();
                x.b_DatePrice = (int)reader["b_DatePrice"];
                list.Add(x);
            }
            con.Close();
            return list;
        }
    }
}