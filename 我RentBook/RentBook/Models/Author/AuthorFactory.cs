using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentBook.Models.Author
{
    public class AuthorFactory
    {
        string myDBConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";

        public string 自動產生a_id()
        {
            string a_id最大值 = "";

            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select max(a_id) as a_id from Author";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                if (reader["a_id"] == null)
                {
                    a_id最大值 = "A00001";
                }
                else
                {
                    a_id最大值 = (string)reader["a_id"];
                }
            }

            reader.Close();
            con.Close();

            int 加號 = Convert.ToInt32(a_id最大值.Substring(1, a_id最大值.Length - 1)) + 1;
            string 新增的a_id = "A" + string.Format("{0:00000}", 加號);
            return 新增的a_id;
        }

        public List<AuthorModel> getByKeyword(string keyword)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "Select * from Author where a_id Like @aid or a_Name Like @aName";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("aid", '%' + keyword + '%');
            cmd.Parameters.AddWithValue("aName", '%' + keyword + '%');
            SqlDataReader reader = cmd.ExecuteReader();

            List<AuthorModel> list = new List<AuthorModel>();
            while (reader.Read())
            {
                AuthorModel a = new AuthorModel();
                a.a_id = (string)reader["a_id"];
                a.a_Name = (string)reader["a_Name"];
                a.a_Image = (string)reader["a_Image"];
                a.a_Birth = ((DateTime)reader["a_Birth"]).ToString("yyyy/MM/dd");
                a.a_Point = (int)reader["a_Point"];
                a.a_Email = (string)reader["a_Email"];
                list.Add(a);
            }

            reader.Close();
            con.Close();

            return list;
        }

        public List<AuthorModel> SeleteAll()
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();
            string tSQL = "select a_id,a_Name,ISNULL(a_Image,'未知') as a_Image,ISNULL(a_Birth,'1900-01-01') as a_Birth,ISNULL(a_Point,0) as a_Point,ISNULL(a_Email,'xxx@RentBook.com') as a_Email from Author";
            SqlCommand cmd = new SqlCommand(tSQL,con);
            SqlDataReader reader = cmd.ExecuteReader();

            List<AuthorModel> list = new List<AuthorModel>();
            while (reader.Read())
            {
                AuthorModel a = new AuthorModel();
                a.a_id = (string)reader["a_id"];
                a.a_Name = (string)reader["a_Name"];
                a.a_Image = (string)reader["a_Image"];
                a.a_Birth = ((DateTime)reader["a_Birth"]).ToString("yyyy/MM/dd");
                a.a_Point = (int)reader["a_Point"];
                a.a_Email = (string)reader["a_Email"];
                list.Add(a);
            }

            reader.Close();
            con.Close();

            return list;
        }

        public void Create(AuthorModel a)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();
            string tSQL = "Insert into Author (a_id,a_Name,a_Image,a_Birth,a_Point,a_Email)Values(@aid,@aName,@aImage,@aBirth,@aPoint,@aEmail)";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("aid", a.a_id);
            cmd.Parameters.AddWithValue("aName", a.a_Name);
            if(a.a_Image == null)
                a.a_Image = "未知";
            cmd.Parameters.AddWithValue("aImage", a.a_Image);
            if (a.a_Birth == null)
                a.a_Birth = "1900-01-01";
            cmd.Parameters.AddWithValue("aBirth", a.a_Birth);
            cmd.Parameters.AddWithValue("aPoint", 0);
            if (a.a_Email == null)
                a.a_Email = "xxx@RentBook.com";
            cmd.Parameters.AddWithValue("aEmail", a.a_Email);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public AuthorModel 要修改的作者資料(string aid)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();
            string tSQL = "select * from Author where a_id=@aid";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("aid", aid);
            SqlDataReader reader = cmd.ExecuteReader();

            AuthorModel aa = new AuthorModel();

            if (reader.Read())
            {
                aa.a_id = (string)reader["a_id"];
                aa.a_Name = (string)reader["a_Name"];
                aa.a_Image = (string)reader["a_Image"];
                aa.a_Birth = ((DateTime)reader["a_Birth"]).ToString("yyyy/MM/dd");
                aa.a_Point = (int)reader["a_Point"];
                aa.a_Email = (string)reader["a_Email"];
            }

            reader.Close();
            con.Close();

            return aa;
        }

        public string 傳回原照片檔名(string aid)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();
            string tSQL = "select a_Image from Author where a_id=@aid";
            SqlCommand cmd = new SqlCommand(tSQL,con);
            cmd.Parameters.AddWithValue("aid", aid);
            SqlDataReader reader = cmd.ExecuteReader();

            string oldfilename = "";

            if (reader.Read())
            {
                oldfilename = (string)reader["a_Image"];
            }

            reader.Close();
            con.Close();
            
            return oldfilename;
        }

        public void Edit(AuthorModel a)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();
            
            string tSQL = "Update Author set a_Name=@aName,a_Birth=@aBirth,a_Email=@aEmail ";
            if (!string.IsNullOrEmpty(a.a_Image))
            tSQL += ",a_Image=@aImage ";
            tSQL += "where a_id=@aid";

            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("aid", a.a_id);
            cmd.Parameters.AddWithValue("aName", a.a_Name);
            cmd.Parameters.AddWithValue("aBirth", a.a_Birth);
            cmd.Parameters.AddWithValue("aEmail", a.a_Email);
            if (!string.IsNullOrEmpty(a.a_Image))
            cmd.Parameters.AddWithValue("aImage", a.a_Image);

            cmd.ExecuteNonQuery();
            
            con.Close();
           
        }
    }
}