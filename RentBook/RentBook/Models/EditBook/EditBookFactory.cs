using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentBook.Models.EditBook
{
    public class EditBookFactory
    {
        string myDBConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";

        public List<EditBookModel> 列出所有書籍資訊()
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select A.b_Image,A.b_id,A.b_Name,A.b_Info,A.b_Type,A.b_DatePrice,A.b_AgeRating,A.p_id + ' ' + B.p_Name as 出版社編號名稱,A.b_Series_yn,A.b_Put_yn From Books A left outer join Publishing B on A.p_id = B.p_id";

            SqlCommand cmd = new SqlCommand(tSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            List<EditBookModel> list = new List<EditBookModel>();

            while (reader.Read())
            {
                EditBookModel eb = new EditBookModel();

                eb.b_Image = (string)reader["b_Image"];
                eb.b_id = (string)reader["b_id"];
                eb.b_Name = (string)reader["b_Name"];
                eb.b_Info = (string)reader["b_Info"];
                eb.b_Type = (string)reader["b_Type"];
                eb.b_DatePrice = (int)reader["b_DatePrice"];
                eb.b_AgeRating = (string)reader["b_AgeRating"];
                eb.出版社編號名稱 = (string)reader["出版社編號名稱"];

                if ((string)reader["b_Series_yn"] == "y")
                    eb.b_Series_yn = "連載中";
                else if ((string)reader["b_Series_yn"] == "n")
                    eb.b_Series_yn = "已完結";

                if ((string)reader["b_Put_yn"] == "y")
                    eb.b_Put_yn = "上架";
                else if ((string)reader["b_Put_yn"] == "n")
                    eb.b_Put_yn = "下架";


                eb.b_ImagePath = "../書籍素材/" + eb.b_Type + "素材/" + eb.b_id + "/" + eb.b_id + "-cover.jpg";
                

                list.Add(eb);
            }

            reader.Close();
            con.Close();

            return list;
        }

        public List<EditBookModel> getByKeyword(string keyword)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select A.b_Image,A.b_id,A.b_Name,A.b_Info,A.b_Type,A.b_DatePrice,A.b_ISBN,A.b_AgeRating,A.p_id + ' ' + B.p_Name as 出版社編號名稱,A.b_Series_yn,A.b_Put_yn From Books A left outer join Publishing B on A.p_id = B.p_id where A.b_id like @bid or A.b_Name like @bName";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bid", '%' + keyword + '%');
            cmd.Parameters.AddWithValue("bName", '%' + keyword + '%');
            SqlDataReader reader = cmd.ExecuteReader();

            List<EditBookModel> list = new List<EditBookModel>();
            while (reader.Read())
            {
                EditBookModel eb = new EditBookModel();
                eb.b_Image = (string)reader["b_Image"];
                eb.b_id = (string)reader["b_id"];
                eb.b_Name = (string)reader["b_Name"];
                eb.b_Info = (string)reader["b_Info"];
                eb.b_Type = (string)reader["b_Type"];
                eb.b_DatePrice = (int)reader["b_DatePrice"];
                eb.b_ISBN = (string)reader["b_ISBN"];
                eb.b_AgeRating = (string)reader["b_AgeRating"];
                eb.出版社編號名稱 = (string)reader["出版社編號名稱"];
                
                if ((string)reader["b_Series_yn"] == "y")
                    eb.b_Series_yn = "連載中";
                else if((string)reader["b_Series_yn"] == "n")
                    eb.b_Series_yn = "已完結";

                if ((string)reader["b_Put_yn"] == "y")
                    eb.b_Put_yn = "上架";
                else if ((string)reader["b_Put_yn"] == "n")
                    eb.b_Put_yn = "下架";

                eb.b_ImagePath = "../書籍素材/" + eb.b_Type + "素材/" + eb.b_id + "/" + eb.b_id + "-cover.jpg";

                list.Add(eb);
            }

            reader.Close();
            con.Close();

            return list;
        }

        public EditBookModel 帶出要修改的書籍資訊(string b_id)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select A.b_Image,A.b_id,A.b_Name,A.b_Info,A.b_Type,A.b_PublishedDate,A.b_DatePrice,A.b_ISBN,A.b_AgeRating,A.p_id + '  ' + B.p_Name as 出版社編號名稱,A.b_Series_yn,A.b_Put_yn From Books A left outer join Publishing B on A.p_id = B.p_id where A.b_id=@bid";

            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bid", b_id);
            SqlDataReader reader = cmd.ExecuteReader();

            EditBookModel eb = new EditBookModel();

            if (reader.Read())
            {
                eb.b_Image = (string)reader["b_Image"];
                eb.b_id = (string)reader["b_id"];
                eb.b_Name = (string)reader["b_Name"];
                eb.b_Info = (string)reader["b_Info"];
                eb.b_Type = (string)reader["b_Type"];
                eb.b_PublishedDate = ((DateTime)reader["b_PublishedDate"]).ToString("yyyy/MM/dd");
                eb.b_DatePrice = (int)reader["b_DatePrice"];
                eb.b_ISBN = (string)reader["b_ISBN"];
                eb.b_AgeRating = (string)reader["b_AgeRating"];
                eb.出版社編號名稱 = (string)reader["出版社編號名稱"];

                if ((string)reader["b_Series_yn"] == "y")
                    eb.b_Series_yn = "連載中";
                else if ((string)reader["b_Series_yn"] == "n")
                    eb.b_Series_yn = "已完結";

                if ((string)reader["b_Put_yn"] == "y")
                    eb.b_Put_yn = "上架";
                else if ((string)reader["b_Put_yn"] == "n")
                    eb.b_Put_yn = "下架"; 
                
                eb.b_ImagePath = "../書籍素材/" + eb.b_Type + "素材/" + eb.b_id + "/" + eb.b_id + "-cover.jpg";
            }

            reader.Close();
            con.Close();

            return eb;
        }

        public string 列出書籍Tags(string b_id)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select * from BooksTags as A inner join Tags as B on A.t_id=B.t_id where b_id=@bid";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bid", b_id);
            SqlDataReader reader = cmd.ExecuteReader();

            string 書籍Tags = "";

            while (reader.Read())
            {
                書籍Tags += "#" + (string)reader["t_Name"] + " ";
            }

            reader.Close();
            con.Close();

            return 書籍Tags;
        }


        public List<string> 傳回出版社編號名稱列表()
        {
            List<string> 出版社編號名稱列表 = new List<string>();

            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select p_Id + '  ' + p_Name as 出版社編號名稱 from Publishing";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                出版社編號名稱列表.Add((string)reader["出版社編號名稱"]);
            }

            reader.Close();
            con.Close();

            return 出版社編號名稱列表;
        }

        // 傳回作者編號名稱 (前端的下拉式選單使用)
        public List<string> 傳回作者編號名稱列表()
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

        // 列出這本書的所有章節
        public List<EditBookModel> 列出書籍所有章節(string b_id)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select * from BooksChapters where b_id=@bid";

            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bid", b_id);
            SqlDataReader reader = cmd.ExecuteReader();

            List<EditBookModel> list = new List<EditBookModel>();

            while (reader.Read())
            {
                EditBookModel eb = new EditBookModel();

                eb.bc_id = (int)reader["bc_id"];
                eb.b_id = (string)reader["b_id"];
                eb.bc_Chapters = (int)reader["bc_Chapters"];
                eb.bc_Content = (string)reader["bc_Content"];

                list.Add(eb);
            }

            reader.Close();
            con.Close();

            return list;
        }

        public EditBookModel 帶出要修改的章節資訊(int bc_id)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select * from BooksChapters as A inner join Books as B on A.b_id=B.b_id where bc_id=@bcid";

            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bcid", bc_id);
            SqlDataReader reader = cmd.ExecuteReader();

            EditBookModel eb = new EditBookModel();

            if (reader.Read())
            {
                eb.bc_id = (int)reader["bc_id"];
                eb.b_id = (string)reader["b_id"];
                eb.bc_Chapters = (int)reader["bc_Chapters"];
                eb.bc_Content = (string)reader["bc_Content"];
                eb.b_Type = (string)reader["b_Type"];
            }

            reader.Close();
            con.Close();

            return eb;
        }

        public List<string> 回傳該本書籍的作者List(string b_id)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select * from BooksAuthor as a inner join Author as b on a.a_id=b.a_id where a.b_id=@bid";

            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bid", b_id);
            SqlDataReader reader = cmd.ExecuteReader();

            EditBookModel eb = new EditBookModel();

            List<string> 本書的作者 = new List<string>();

            while (reader.Read())
            {
                eb.a_id = (string)reader["a_id"];
                eb.a_Name = (string)reader["a_Name"];
                string s = eb.a_id + "  " + eb.a_Name;
                本書的作者.Add(s);
            }

            reader.Close();
            con.Close();

            return 本書的作者;
        }

        //----------------- 以上都是把資料呈現到 View 的 function ---------------
        //----------------- 以下開始存入資料 ------------------------------------

        // Update Books 資料表
        public void SaveBookData_Books(EditBookModel eb)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "update Books set ";
            tSQL += "b_Name=@bName,";
            tSQL += "b_Info=@bInfo,";
            tSQL += "b_Image=@bImage,";
            tSQL += "b_PublishedDate=@bPublishedDate,";
            tSQL += "b_DatePrice=@bDatePrice,";
            tSQL += "b_ISBN=@bISBN,";
            tSQL += "b_AgeRating=@bAgeRating,";
            tSQL += "b_Series_yn=@bSeries,";
            tSQL += "b_Put_yn=@bPut,";
            tSQL += "p_id=@pid";
            tSQL += " where b_id=@bid";

            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bid", eb.b_id);
            cmd.Parameters.AddWithValue("bName", eb.b_Name);
            cmd.Parameters.AddWithValue("bInfo", eb.b_Info);
            cmd.Parameters.AddWithValue("bImage", eb.b_Image);
            cmd.Parameters.AddWithValue("bPublishedDate", eb.b_PublishedDate);
            cmd.Parameters.AddWithValue("bDatePrice", eb.b_DatePrice);
            cmd.Parameters.AddWithValue("bISBN", eb.b_ISBN);
            cmd.Parameters.AddWithValue("bAgeRating", eb.b_AgeRating);
            cmd.Parameters.AddWithValue("pid", eb.p_id);
            cmd.Parameters.AddWithValue("bPut", eb.b_Put_yn);

            // 連載情況
            char 連載情況;
            if (eb.b_Series_yn == "連載中")
            {
                連載情況 = 'y';
                cmd.Parameters.AddWithValue("bSeries", 連載情況);
            }
            else if (eb.b_Series_yn == "已完結")
            {
                連載情況 = 'n';
                cmd.Parameters.AddWithValue("bSeries", 連載情況);
            }

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public string 傳回原書籍封面照片檔名(string bid)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();
            string tSQL = "select b_Image from Books where b_id=@bid";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bid", bid);
            SqlDataReader reader = cmd.ExecuteReader();

            string oldfilename = "";

            if (reader.Read())
            {
                oldfilename = (string)reader["b_Image"];
            }

            reader.Close();
            con.Close();

            return oldfilename;
        }

        // 儲存新封面圖片使用
        public string 書籍封面圖片命名(EditBookModel eb)
        {
            // 取得副檔名
            int point = eb.Image.FileName.LastIndexOf(".");
            string extention = eb.Image.FileName.Substring(point, eb.Image.FileName.Length - point);
            // 命名封面檔名
            string photoName = eb.b_id + "-cover" + extention;

            return photoName;
        }

        // 解析出版社資料(傳入資料庫用)
        public string 出版社資料解析成編號(string PublishedIdName)
        {
            string[] 解析結果 = PublishedIdName.Split(' ');
            return 解析結果[0];
        }

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

        // 儲存 Tags
        public void 移除此書籍的標籤(string b_id)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "Delete from BooksTags where b_id=@bid";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bid", b_id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void 儲存到標籤資料表(EditBookModel eb)
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

            for (int i = 0; i < eb.Tags.Count; i++)
            {
                bool check = false;
                for (int j = 0; j < listTag.Count; j++)
                {
                    if (eb.Tags[i] == listTag[j])
                    {
                        check = true;
                    }
                }
                if (check == false)
                {
                    if (eb.Tags[i] != "")
                    {
                        string tSQL2 = "Insert into Tags (t_Name)Values('" + eb.Tags[i] + "')";
                        cmd2.CommandText = tSQL2;
                        cmd2.ExecuteNonQuery();
                    }
                }
            }

            // 找出標籤序號
            SqlCommand cmd3 = new SqlCommand();
            cmd3.Connection = con;

            List<int> 標籤序號 = new List<int>();

            for (int i = 0; i < eb.Tags.Count; i++)
            {
                string tSQL3 = "select t_id from Tags where t_Name='" + eb.Tags[i] + "'";
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
                string tSQL4 = "Insert into BooksTags (b_id,t_id)Values('" + eb.b_id + "'," + i + ")";
                cmd4.CommandText = tSQL4;
                cmd4.ExecuteNonQuery();
            }

            con.Close();
        }

        // 儲存作者
        public void 移除此書籍的作者(string b_id)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "Delete from BooksAuthor where b_id=@bid";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bid", b_id);

            cmd.ExecuteNonQuery();
            con.Close();
        }
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
        // 將資料儲存到書籍作者資料表
        public void SaveBooksAuthor(EditBookModel eb)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            List<string> 作者編號陣列 = 作者資料陣列解析成編號(eb.AuthorIdName);

            string SQL = "";
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con;

            foreach (string a_id in 作者編號陣列)
            {
                SQL = "Insert into BooksAuthor (b_id,a_id)Values('" + eb.b_id + "','" + a_id + "')";
                cmd.CommandText = SQL;
                cmd.ExecuteNonQuery();
            }

            con.Close();
        }

        // 儲存修改的章節標題
        public void SaveEditBookChapters(EditBookModel eb)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "Update BooksChapters set bc_Content=@bcContent where bc_id=@bcid";
            SqlCommand cmd = new SqlCommand(tSQL,con);
            cmd.Parameters.AddWithValue("bcid", eb.bc_id);
            cmd.Parameters.AddWithValue("bcContent", eb.bc_Content);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        // 刪除所有該章節的檔案
        public List<string> 回傳所有該章節的檔名(EditBookModel eb)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select bf_FileName from BooksFiles where bc_id=@bcid";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bcid", eb.bc_id);
            SqlDataReader reader = cmd.ExecuteReader();

            List<string> list = new List<string>();

            while (reader.Read())
            {
                list.Add((string)reader["bf_FileName"]);
            }

            con.Close();

            return list;
        }

        public void 刪除該章節在資料庫裡的所有的檔名(int bc_id)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "delete from BooksFiles where bc_id=@bcid";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bcid", bc_id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void 儲存該章節所有檔名到資料庫(EditBookModel eb)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("bcid", eb.bc_id);
            cmd.Parameters.AddWithValue("bfFileName", eb.bc_id);

            foreach (string Files in eb.FilesName)
            {
                tSQL = "insert into BooksFiles (bc_id,bf_FileName)Values('" + eb.bc_id +"','" + Files + "')";
                cmd.CommandText = tSQL;
                cmd.ExecuteNonQuery();
            }

            con.Close();
        }

        public string 回傳書籍章節檔案副檔名(HttpPostedFileBase file)
        {
            // 取得副檔名
            int point = file.FileName.LastIndexOf(".");
            string extention = file.FileName.Substring(point, file.FileName.Length - point);

            return extention;
        }
    }
}