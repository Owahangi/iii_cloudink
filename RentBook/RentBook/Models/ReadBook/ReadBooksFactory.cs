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

        

        public List<string> ReadfileContent(ReadBooksModel rb)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            //找出章節檔案名稱
            string tSQL = "select bf_FileName from BooksFiles as b inner join BooksChapters as c on b.bc_id=c.bc_id where c.b_id=@Searchb_id and c.bc_Chapters=@Searchc_Chapters";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("Searchb_id", rb.b_id);
            cmd.Parameters.AddWithValue("Searchc_Chapters", rb.bc_Chapters);
            SqlDataReader reader = cmd.ExecuteReader();

            string bf_FileName = "";

            if (reader.Read())
            {
                bf_FileName = (string)reader["bf_FileName"];
            }

            reader.Close();
            con.Close();

            List<string> 書籍內容 = new List<string>();

            if (rb.b_id != "")
            {
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/書籍素材/小說素材/" + rb.b_id + "/" + rb.b_id + "-" + rb.bc_Chapters + "/" + bf_FileName)))
                {

                }
                else
                {
                    StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/書籍素材/小說素材/" + rb.b_id + "/" + rb.b_id + "-" + rb.bc_Chapters + "/" + bf_FileName), System.Text.Encoding.UTF8);
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

        public List<string> ReadComicBookfileContent(ReadBooksModel rb)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string where條件 = rb.b_id + "-" + rb.bc_Chapters + "%";

            // 找出章節檔名
            //string tSQL = "select c_FileName from BooksChapters where c_FileName like '123-1%'";
            string tSQL = "select bf_FileName from BooksFiles where bf_FileName like @Where條件";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("Where條件", where條件);
            SqlDataReader reader = cmd.ExecuteReader();

            List<string> 章節圖片檔名 = new List<string>();

            while (reader.Read())
            {
                章節圖片檔名.Add((string)reader["bf_FileName"]);
            }

            reader.Close();
            con.Close();

            return 章節圖片檔名;
        }

        public int 回傳書籍最大章節數量(string b_id)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select max(bc_Chapters) as bc_Chapters from BooksChapters where b_id=@bid";
            SqlCommand cmd = new SqlCommand(tSQL,con);
            cmd.Parameters.AddWithValue("@bid", b_id);
            SqlDataReader reader = cmd.ExecuteReader();

            int 書籍最大章節數 = 0;

            if (reader.Read())
            {
                書籍最大章節數 = (int)reader["bc_Chapters"];
            }

            reader.Close();
            con.Close();

            return 書籍最大章節數;
        }

        public string 傳回目前章節標題(string b_id,int bc_Chapters)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select bc_Content from BooksChapters where b_id=@bid and bc_Chapters=@bcChapters";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("@bid", b_id);
            cmd.Parameters.AddWithValue("@bcChapters", bc_Chapters);
            SqlDataReader reader = cmd.ExecuteReader();

            string 書籍章節標題 = "";

            if (reader.Read())
            {
                書籍章節標題 = (string)reader["bc_Content"];
            }

            reader.Close();
            con.Close();

            return 書籍章節標題;
        }
    }
}