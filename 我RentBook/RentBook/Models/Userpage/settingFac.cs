using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace RentBook.Models.Userpage
{
    public class settingFac
    {
        public void uploadImg(string photoName,string userEmail)
        {
            string sql = @"
update Member
set m_Image=@photoName
where m_Email=@userEmail;";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("photoName",photoName));
            paras.Add(new SqlParameter("userEmail",userEmail));
            executeSql(sql, paras);
        }

        public void editMySetting(settingClass s,string userMail)
        {
            string sql = @"
update Member
set m_Name=@mName,m_Alias=@mAlias,m_Gender=@mGender,m_Intro=@mIntro
where m_Email=@userMail;
update SystemAccount
set s_Pwd=@userPwd
where s_id=@userMail
";//m_id=@userMid
            List<SqlParameter> paras = new List<SqlParameter>();
            //paras.Add(new SqlParameter("mEmail", (object)s.m_Email));//m_Email=@mEmail,
           /* if (s.s_Pwd == null)
            {
                s.s_Pwd = "";
            }*/
            paras.Add(new SqlParameter("userPwd", (object)s.s_Pwd));
            paras.Add(new SqlParameter("mName", (object)s.m_Name));
            if (s.m_Alias == null)
            {
                s.m_Alias = "";
            }
            paras.Add(new SqlParameter("mAlias", (object)s.m_Alias));
            paras.Add(new SqlParameter("mGender", (object)s.m_Gender));
            if (s.m_Intro == null)
            {
                s.m_Intro = "";
            }
            paras.Add(new SqlParameter("mIntro", (object)s.m_Intro));
            paras.Add(new SqlParameter("userMail", userMail));
            //paras.Add(new SqlParameter("userMid", s.m_id));
            executeSql(sql, paras);
        }


        public List<settingClass> getMySetting(string userEmail)
        {
            string sql = @"
select m_id,m_Image,m_Name,m_Gender,m_Alias,m_Email,s.s_Pwd,m_Intro from Member as m
inner join SystemAccount as s on m.m_Email=s.s_id
where m_Email=@userEmail;
";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("userEmail", userEmail));
            return getBySql(sql, paras);
        }
        private List<settingClass> getBySql(string sql, List<SqlParameter> paras)
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
            List<settingClass> list = new List<settingClass>();
            while (reader.Read())
            {
                settingClass x = new settingClass();
                x.m_id = reader["m_id"].ToString();
                x.m_Image = reader["m_Image"].ToString();
                x.m_Name = reader["m_Name"].ToString();
                x.m_Gender = reader["m_Gender"].ToString();
                x.m_Alias = reader["m_Alias"].ToString();
                if (x.m_Alias == null)
                {
                    x.m_Alias = "";
                }
                x.m_Email = reader["m_Email"].ToString();
                x.s_Pwd = reader["s_Pwd"].ToString();
                x.m_Intro = reader["m_Intro"].ToString();
                if (x.m_Intro == null)
                {
                    x.m_Intro = "";
                }
                if (x.m_Image == "未知")
                {
                    x.m_Image = "Default.jpg";
                }
                list.Add(x);
            }
            con.Close();
            return list;
        }
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
        }
    }
}