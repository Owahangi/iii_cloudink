using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentBook.Models.AddChapters
{
    public class AddChaptersFactory
    {
        string myDBConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";

        public string 回傳書籍章節檔案副檔名(HttpPostedFileBase file)
        {
            // 取得副檔名
            int point = file.FileName.IndexOf(".");
            string extention = file.FileName.Substring(point, file.FileName.Length - point);

            return extention;
        }

        // 撈出此本書的最新的章節
        public int 目前最新章節(AddBookChaptersModel ac)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select max(bc_Chapters) as bc_Chapters from BooksChapters where b_id = @bId";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bId", ac.b_id);
            SqlDataReader reader = cmd.ExecuteReader();

            int 目前最新章節數 = 0;

            if (ac.b_id != null)
            {
                if (reader.Read())
                {
                    目前最新章節數 = (int)reader["bc_Chapters"];
                }
            }

            reader.Close();
            con.Close();

            return 目前最新章節數;
        }

        // 將資料儲存到 BookOutline 與 BooksChapters 資料表
        public void 儲存章節標題及檔名(AddBookChaptersModel ac)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            // 新增到書籍大綱資料表
            string tSQL = "Insert into BooksChapters (b_id,bc_Chapters,bc_Content)Values(@bId,@bcChapters,@bcContent)";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bId", ac.b_id);
            cmd.Parameters.AddWithValue("bcChapters", ac.bc_Chapters);
            cmd.Parameters.AddWithValue("bcContent", ac.bc_Content);

            cmd.ExecuteNonQuery();


            // 撈回 BooksChapters 資料表，目前自動編號的最大值 (要塞到 BooksFiles 資料表使用的)
            string tSQL1 = "select max(bc_id) as bc_id from BooksChapters";
            SqlCommand cmd1 = new SqlCommand(tSQL1, con);
            SqlDataReader reader = cmd1.ExecuteReader();

            int maxbc_id = 0;

            if (reader.Read())
            {
                maxbc_id = (int)reader["bc_id"];
            }

            reader.Close();


            // 新增到書籍章節資料表
            string tSQL2 = "";
            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = con;

            foreach (string Files in ac.FilesName)
            {
                tSQL2 = "Insert into BooksFiles (bc_id,bf_FileName)Values('" + maxbc_id + "','" + Files + "')";
                cmd2.CommandText = tSQL2;

                cmd2.ExecuteNonQuery();

            }

            con.Close();
        }
    }
}