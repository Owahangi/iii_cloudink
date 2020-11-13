using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.IO;

namespace RentBook.Models
{
    public class ReadBooksFactory
    {
        string myDBConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";

        public string Readfilename(string b_id) {
            string filename = "";

            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();
            string tSQL = "select c_FileName from BooksChapters where b_id = @Searchb_id";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("Searchb_id", b_id);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                filename = (string)reader["c_FileName"];
            }

            reader.Close();
            con.Close();

            return filename;
        }

        public List<string> ReadfileContent(string b_id)
        {
            //找出書籍名稱
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();
            string tSQL = "select b_Name from Books where b_id = @Searchb_id";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("Searchb_id", b_id);
            SqlDataReader reader = cmd.ExecuteReader();

            string bookname = "";

            if (reader.Read()) {
                bookname = (string)reader["b_Name"];
            }

            reader.Close();
            con.Close();

            List<string> 書籍內容 = new List<string>();

            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/書籍素材/小說素材/星辰變/1. 第一章 秦羽.txt")))
            {

            }
            else
            {
                StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/書籍素材/小說素材/星辰變/1. 第一章 秦羽.txt"), System.Text.Encoding.UTF8);
                while (sr.Peek() >= 0)
                {
                    書籍內容.Add(sr.ReadLine());
                }
                //String input = sr.ReadLine();
                sr.Close();
            }

            return 書籍內容;

        }
    }
}