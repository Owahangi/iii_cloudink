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

        public string Readfilename(string b_id)
        {
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

        public List<string> ReadfileContent(string b_id,string c_FileName)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            //找出書籍名稱
            string tSQL = "select b_Name from Books where b_id = @Searchb_id";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("Searchb_id", b_id);
            SqlDataReader reader = cmd.ExecuteReader();

            string bookname = "";

            if (reader.Read())
            {
                bookname = (string)reader["b_Name"];
            }

            reader.Close();

            //找出章節名稱
            string tSQL1 = "select c_FileName from BooksChapters where b_id = @Searchb_id and c_FileName = @Searchc_FileName";
            SqlCommand cmd1 = new SqlCommand(tSQL1, con);
            cmd1.Parameters.AddWithValue("Searchb_id", b_id);
            cmd1.Parameters.AddWithValue("Searchc_FileName", c_FileName);
            SqlDataReader reader1 = cmd1.ExecuteReader();

            string chapters = "";

            if (reader1.Read())
            {
                chapters = (string)reader1["c_FileName"];
            }
            reader1.Close();
            con.Close();

            List<string> 書籍內容 = new List<string>();

            if (bookname != "")
            {
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/書籍素材/小說素材/" + bookname + "/" + chapters + ".txt")))
                {

                }
                else
                {
                    StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/書籍素材/小說素材/" + bookname + "/" + chapters + ".txt"), System.Text.Encoding.UTF8);
                    while (sr.Peek() >= 0)
                    {
                        書籍內容.Add(sr.ReadLine());
                    }
                    //String input = sr.ReadLine();
                    sr.Close();
                }

                return 書籍內容;
            }
            else
            {
                書籍內容.Add("沒有書籍資料，請稍後在試");
                return 書籍內容;
            }
        }

        //public List<string> ReadComicBookfileContent(string b_id, string c_FileName)
        //{


        //}

    }
}