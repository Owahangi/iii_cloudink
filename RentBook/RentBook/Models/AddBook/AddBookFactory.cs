using RentBook.Models.AddBook;
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
            SqlCommand cmd = new SqlCommand(tSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                if (reader["b_id"] == null)
                {
                    b_id最大值 = "B00001";
                }
                else
                {
                    b_id最大值 = (string)reader["b_id"];
                }
            }

            reader.Close();
            con.Close();

            int 加號 = Convert.ToInt32(b_id最大值.Substring(1, b_id最大值.Length - 1)) + 1;
            string 新增的b_id = "B" + string.Format("{0:00000}", 加號);
            return 新增的b_id;
        }

        public string 書籍封面圖片命名(AddBooks b)
        {
            // 取得副檔名
            int point = b.Image.FileName.IndexOf(".");
            string extention = b.Image.FileName.Substring(point, b.Image.FileName.Length - point);
            // 命名封面檔名
            string photoName = b.b_id + "-cover" + extention;

            return photoName;
        }

        // 將 Tags 拆成陣列
        public List<string> Tags轉成陣列(string Tags)
        {
            string[] arrsplit = Tags.Split('#');
            List<string> returnlist = new List<string>();
            foreach (string a in arrsplit)
            {
                if (a != "")
                {
                    returnlist.Add(a.Trim());
                }
            }

            return returnlist;
        }


        // 傳回出版社名稱 (前端的下拉式選單使用)
        public List<string> 傳回出版社編號名稱()
        {
            List<string> 出版社編號名稱 = new List<string>();

            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select p_Id + ' ' + p_Name as 出版社編號名稱 from Publishing";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                if (reader["出版社編號名稱"] != null)
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

            string tSQL = "select a_Id + ' ' + a_Name as 作者編號名稱 from Author";
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
        // 將資料儲存到 Books 資料表
        public void Create(AddBooks b)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            //Insert into Books (b_id,b_Name,b_Info,b_Image,b_Type,b_PublishedDate,b_DatePrice,b_ISBN,b_AgeRating,p_id)Values(1234,'aaa','bbb','c','d','1996/09/07','1','g',1,1)

            string tSQL = "Insert into Books (b_id,b_Name,b_Info,b_Image,b_Type,b_PublishedDate,b_DatePrice,b_ISBN,b_AgeRating,b_Series_yn,b_Put_yn,p_id)Values(" +
                "@bid,@bName,@bInfo,@bImage,@bType,@bPublishedDate,@bDatePrice,@bISBN,@bAgeRating,@bSeries,'n',@pID)";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bid", b.b_id);
            cmd.Parameters.AddWithValue("bName", b.b_Name);
            cmd.Parameters.AddWithValue("bInfo", b.b_Info);
            cmd.Parameters.AddWithValue("bImage", b.b_Image);
            cmd.Parameters.AddWithValue("bType", b.b_Type);
            cmd.Parameters.AddWithValue("bPublishedDate", b.b_PublishedDate);
            cmd.Parameters.AddWithValue("bDatePrice", b.b_DatePrice);
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
        public void CreateBA(EditBookModel ba)
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

        // 將資料儲存到 BookOutline 與 BooksChapters 資料表
        public void 儲存章節標題及檔名(AddBooks b, AddBooksFiles bf, AddBooksChapters bc)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            // 新增到書籍章節資料表
            string tSQL = "Insert into BooksChapters (b_id,bc_Chapters,bc_Content)Values(@bId,@bcChapters,@bcContent)";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bId", b.b_id);
            cmd.Parameters.AddWithValue("bcChapters", bc.bc_Chapters);
            cmd.Parameters.AddWithValue("bcContent", bc.bc_Content);

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


            // 新增到書籍檔案資料表
            string tSQL2 = "";
            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = con;

            foreach (string Files in bf.FilesName)
            {
                tSQL2 = "Insert into BooksFiles (bc_id,bf_FileName)Values('" + maxbc_id + "','" + Files + "')";
                cmd2.CommandText = tSQL2;
                cmd2.ExecuteNonQuery();
            }

            con.Close();
        }

        public void 儲存到標籤資料表(AddBooksTags bt)
        {
            // 找出目前現有的 Tags 資料表
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL1 = "select t_Name from Tags";
            SqlCommand cmd1 = new SqlCommand(tSQL1, con);
            SqlDataReader reader = cmd1.ExecuteReader();

            List<string> listTag = new List<string>();

            while (reader.Read())
            {
                listTag.Add((string)reader["t_Name"]);
            }

            reader.Close();


            // 將標籤新增到 Tags 資料表 (只新增新的標籤 重複的標籤不新增)
            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = con;

            for (int i = 0; i < bt.Tags.Count; i++)
            {
                bool check = false;
                for (int j = 0; j < listTag.Count; j++)
                {
                    if (bt.Tags[i] == listTag[j])
                    {
                        check = true;
                    }
                }
                if (check == false)
                {
                    if (bt.Tags[i] != "")
                    {
                        string tSQL2 = "Insert into Tags (t_Name)Values('" + bt.Tags[i] + "')";
                        cmd2.CommandText = tSQL2;
                        cmd2.ExecuteNonQuery();
                    }
                }
            }

            // 找出標籤序號
            SqlCommand cmd3 = new SqlCommand();
            cmd3.Connection = con;

            List<int> 標籤序號 = new List<int>();

            for (int i = 0; i < bt.Tags.Count; i++)
            {
                string tSQL3 = "select t_id from Tags where t_Name='" + bt.Tags[i] + "'";
                cmd3.CommandText = tSQL3;
                SqlDataReader reader1 = cmd3.ExecuteReader();
                if (reader1.Read())
                {
                    標籤序號.Add((int)reader1["t_id"]);
                }
                reader1.Close();
            }


            // 將標籤序號 新增到 BooksTags 資料表
            SqlCommand cmd4 = new SqlCommand();
            cmd4.Connection = con;

            foreach (int i in 標籤序號)
            {
                string tSQL4 = "Insert into BooksTags (b_id,t_id)Values('" + bt.b_id + "'," + i + ")";
                cmd4.CommandText = tSQL4;
                cmd4.ExecuteNonQuery();
            }

            con.Close();
        }

        // 撈出此本書的最新的章節
        public int 目前最新章節(AddBooks b)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select max(bc_Chapters) as bc_Chapters from BooksChapters where b_id = @bId";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bId", b.b_id);
            SqlDataReader reader = cmd.ExecuteReader();

            int 目前最新章節數 = 0;

            if (b.b_id != null)
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

        public string 回傳書籍章節檔案副檔名(HttpPostedFileBase file)
        {
            // 取得副檔名
            int point = file.FileName.LastIndexOf(".");
            string extention = file.FileName.Substring(point, file.FileName.Length - point);

            return extention;
        }

        //public void 更改實體路徑檔案名稱(Books b, BooksChapters bc, string path)
        //{
        //    // 當前目錄
        //    string FolderPath = path;
        //    DirectoryInfo di = new DirectoryInfo(FolderPath);

        //    // 取得所有檔案
        //    FileInfo[] fiArray = di.GetFiles();

        //    List<string> PathAllFileName = new List<string>();


        //}
    }
}