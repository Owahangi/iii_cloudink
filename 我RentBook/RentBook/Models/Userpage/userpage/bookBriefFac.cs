using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace RentBook.Models.Userpage.userpage
{
    public class bookBriefFac
    {
        public List<bookBriefClass> getBookInfo(string userEmail)
        {
            string sql = @"select b.b_Type,b.b_id,b.b_Image,b.b_Name,a.a_Name
from ((MemberShopDetail as msd
inner join Books as b on msd.b_id=b.b_id)
inner join BooksAuthor as ba on ba.b_id=b.b_id)
inner join Author as a on a.a_id=ba.a_id
where m_id=(select m_id from member where m_Email=@m_Email)
order by msd.msd_DateTime desc
";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("m_Email", userEmail));
            return getBySql(sql, paras);
        }
        private List<bookBriefClass> getBySql(string sql, List<SqlParameter> paras)
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
            List<bookBriefClass> list = new List<bookBriefClass>();
            while (reader.Read())
            {
                bookBriefClass x = new bookBriefClass();
                x.b_Type = reader["b_Type"].ToString();
                x.b_id = reader["b_id"].ToString();
                x.b_Image = reader["b_Image"].ToString();
                x.b_Name = reader["b_Name"].ToString();
                x.a_Name = reader["a_Name"].ToString();
                if (x.b_Image == "未知")
                {
                    x.b_Image = "Default.jpg";
                }
                list.Add(x);
            }
            con.Close();
            return list;
        }
    }
}