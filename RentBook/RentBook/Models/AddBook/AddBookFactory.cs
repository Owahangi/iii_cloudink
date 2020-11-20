using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace RentBook.Models
{
    public class AddBookFactory
    {
        string myDBConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";

        // 自動產生書籍編號
        public string 自動產生b_id()
        {
            string b_id最大值 = "";

            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select max(b_id) as b_id from Books";
            SqlCommand cmd = new SqlCommand(tSQL,con);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                b_id最大值 = (string)reader["b_id"];
            }

            reader.Close();
            con.Close();

            int 加號 = Convert.ToInt32(b_id最大值.Substring(1, b_id最大值.Length - 1)) + 1;
            string 新增的b_id = "B" + string.Format("{0:00000}", 加號);
            return 新增的b_id;
        }

        public string 取得圖片附檔名(Books b)
        {
            // 取得副檔名
            int point = b.Image.FileName.IndexOf(".");
            string extention = b.Image.FileName.Substring(point, b.Image.FileName.Length - point);
            // 命名封面檔名
            string photoName = b.b_id + "-cover" + extention;

            return photoName;
        }

        // 傳回出版社名稱 (前端的下拉式選單使用)
        public List<string> 傳回出版社編號名稱() {
            List<string> 出版社編號名稱 = new List<string>();

            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select p_Id + '  ' + p_Name as 出版社編號名稱 from Publishing";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                出版社編號名稱.Add((string)reader["出版社編號名稱"]);
            }

            reader.Close();
            con.Close();

            return 出版社編號名稱;
        }

        // 傳回作者編號名稱 (前端的下拉式選單使用)
        public List<string> 傳回作者編號名稱()
        {
            List<string> 作者編號名稱 = new List<string>();

            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select a_Id + '  ' + a_Name as 作者編號名稱 from Author";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                作者編號名稱.Add((string)reader["作者編號名稱"]);
            }

            reader.Close();
            con.Close();

            return 作者編號名稱;
        }

        // 解析出版社資料(傳入資料庫用)
        public string 出版社資料解析成編號(string PublishedIdName)
        {
            string[] 解析結果 = PublishedIdName.Split(' ');
            return 解析結果[0];
        }

        // 解析作者資料(傳入資料庫用)
        public List<string> 作者資料陣列解析成編號(string[] AuthorIdName)
        {
            List<string> 解析結果 = new List<string>();

            foreach (string a in AuthorIdName)
            {
                string[] 解析 = a.Split(' ');
                解析結果.Add(解析[0]);
            }

            return 解析結果;
        }

        // 新增書籍基本資料
        // 使用資料表：Books、BooksAuthor
        public void Create(Books b)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            //Insert into Books (b_id,b_Name,b_Info,b_Image,b_Type,b_PublishedDate,b_HourPrice,b_ISBN,b_AgeRating,p_id)Values(1234,'aaa','bbb','c','d','1996/09/07','1','g',1,1)

            string tSQL = "Insert into Books (b_id,b_Name,b_Info,b_Image,b_Type,b_PublishedDate,b_HourPrice,b_ISBN,b_AgeRating,b_Series_yn,p_id)Values(" +
                "@bid,@bName,@bInfo,@bImage,@bType,@bPublishedDate,@bHourPrice,@bISBN,@bAgeRating,@bSeries,@pID)";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bid", b.b_id);
            cmd.Parameters.AddWithValue("bName", b.b_Name);
            cmd.Parameters.AddWithValue("bInfo", b.b_Info);
            cmd.Parameters.AddWithValue("bImage", b.b_Image);
            cmd.Parameters.AddWithValue("bType", b.b_Type);
            cmd.Parameters.AddWithValue("bPublishedDate", b.b_PublishedDate);
            cmd.Parameters.AddWithValue("bHourPrice", b.b_HourPrice);
            cmd.Parameters.AddWithValue("bISBN", b.b_ISBN);
            cmd.Parameters.AddWithValue("bAgeRating", b.b_AgeRating);

            // 連載情況
            char 連載情況;
            if (b.b_Series_yn == "連載中")
            {
                連載情況 = 'y';
                cmd.Parameters.AddWithValue("bSeries", 連載情況);

            }
            else if (b.b_Series_yn == "已完結")
            {
                連載情況 = 'n';
                cmd.Parameters.AddWithValue("bSeries", 連載情況);
            }
            cmd.Parameters.AddWithValue("pID", b.p_id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        // 將資料儲存到書籍作者資料表
        public void CreateBA(BooksAuthor ba)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            List<string> 作者編號陣列 = 作者資料陣列解析成編號(ba.AuthorIdName);

            string SQL = "";
            SqlCommand cmd = new SqlCommand();
            
            cmd.Connection = con;

            foreach (string a_id in 作者編號陣列)
            {
                SQL = "Insert into BooksAuthor (b_id,a_id)Values('" + ba.b_id + "','" + a_id + "')";
                cmd.CommandText = SQL;
                cmd.ExecuteNonQuery();
            }

            con.Close();
        }

        public void 儲存章節標題及檔名 (Books b,BooksChapters bc,BookOutline bo)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            // 新增到書籍大綱資料表
            string tSQL = "Insert into BookOutline (b_id,boh_Content)Values('" + b.b_id +"','" + bo.boh_Content + "')";
            SqlCommand cmd = new SqlCommand(tSQL,con);

            cmd.ExecuteNonQuery();

            // 新增到書籍章節資料表
            string tSQL1 = "Insert into BooksChapters (b_id,c_FileName)Values('" + b.b_id + "','" + bc.c_FileName + "')";
            con.Close();
        }

        // 撈出此本書的最新的章節
        public int 目前最新章節 (Books b)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select max(c_Chapters) as c_Chapters from BooksChapters where b_id = '" + b.b_id +"'";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            int 目前最新章節數 = 0;

            if (reader.Read())
            {
                目前最新章節數 = (int)reader["c_Chapters"];
            }

            reader.Close();
            con.Close();

            return 目前最新章節數;
        }


        //public string 取得小說章節檔名(Books b,BooksChapters bc)
        //{
        //    if (bc.Files.Count() > 0)
        //    {
        //        foreach (HttpPostedFileBase uploadFile in bc.Files)
        //        {
        //            if (uploadFile.ContentLength > 0)
        //            {
        //                int i = 1;
        //                // 命名章節檔名
        //                string ChapterName = b.b_id + "-" + i + ".txt";

        //                uploadFile.SaveAs(Server.MapPath("../書籍素材/小說素材/" + b.b_id + "/" + ChapterName));

        //                i++;
        //            }
        //        }
        //    }


        //    return ChapterName;
        //}
    }
}