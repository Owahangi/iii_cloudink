using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentBook.Models.Publishing
{
    public class PublishingFactory
    {
        string myDBConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";

        public string 自動產生p_id()
        {
            string p_id最大值 = "";

            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select max(p_id) as p_id from Publishing";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                if (reader["p_id"] == null)
                {
                    p_id最大值 = "P00001";
                }
                else
                {
                    p_id最大值 = (string)reader["p_id"];
                }
            }

            reader.Close();
            con.Close();

            int 加號 = Convert.ToInt32(p_id最大值.Substring(1, p_id最大值.Length - 1)) + 1;
            string 新增的p_id = "P" + string.Format("{0:00000}", 加號);
            return 新增的p_id;
        }

        public List<PublishingModel> getByKeyword(string keyword)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "Select * from Publishing where p_id Like @pid or p_Name Like @pName";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("pid", '%' + keyword + '%');
            cmd.Parameters.AddWithValue("pName", '%' + keyword + '%');
            SqlDataReader reader = cmd.ExecuteReader();

            List<PublishingModel> list = new List<PublishingModel>();
            while (reader.Read())
            {
                PublishingModel p = new PublishingModel();
                p.p_id = (string)reader["p_id"];
                p.p_Name = (string)reader["p_Name"];
                p.p_Address = (string)reader["p_Address"];
                
                list.Add(p);
            }

            reader.Close();
            con.Close();

            return list;
        }

        public List<PublishingModel> SeleteAll()
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();
            string tSQL = "select p_id,p_Name,p_Address from Publishing";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            List<PublishingModel> list = new List<PublishingModel>();
            while (reader.Read())
            {
                PublishingModel p = new PublishingModel();
                p.p_id = (string)reader["p_id"];
                p.p_Name = (string)reader["p_Name"];
                p.p_Address = (string)reader["p_Address"];
                
                list.Add(p);
            }

            reader.Close();
            con.Close();

            return list;
        }

        public void Create(PublishingModel p)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();
            string tSQL = "Insert into Publishing (p_id,p_Name,p_Address)Values(@pid,@pName,@pAddress)";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("pid", p.p_id);
            cmd.Parameters.AddWithValue("pName", p.p_Name);
            cmd.Parameters.AddWithValue("pAddress", p.p_Address);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public PublishingModel 要修改的出版社資料(string pid)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();
            string tSQL = "select * from Publishing where p_id=@pid";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("pid", pid);
            SqlDataReader reader = cmd.ExecuteReader();

            PublishingModel p = new PublishingModel();

            if (reader.Read())
            {
                p.p_id = (string)reader["p_id"];
                p.p_Name = (string)reader["p_Name"];
                p.p_Address = (string)reader["p_Address"];
            }

            reader.Close();
            con.Close();

            return p;
        }

        public void Edit(PublishingModel p)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "Update Publishing set p_Name=@pName,p_Address=@pAddress where p_id=@pid";
            
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("pid", p.p_id);
            cmd.Parameters.AddWithValue("pName", p.p_Name);
            cmd.Parameters.AddWithValue("pAddress", p.p_Address);
            
            cmd.ExecuteNonQuery();

            con.Close();

        }
    }
}