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

        public List<EditBookList> 列出所有書籍資訊()
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select A.b_Image,A.b_id,A.b_Name,A.b_Type,A.b_DatePrice,A.b_AgeRating,A.p_id + ' ' + B.p_Name as 出版社編號名稱,A.b_Series_yn From Books A left outer join Publishing B on A.p_id = B.p_id";

            SqlCommand cmd = new SqlCommand(tSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            List<EditBookList> list = new List<EditBookList>();

            while (reader.Read())
            {
                EditBookList eb = new EditBookList();

                eb.b_Image = (string)reader["b_Image"];
                eb.b_id = (string)reader["b_id"];
                eb.b_Name = (string)reader["b_Name"];
                eb.b_Type = (string)reader["b_Type"];
                eb.b_DatePrice = (int)reader["b_DatePrice"];
                eb.b_AgeRating = (string)reader["b_AgeRating"];
                eb.出版社編號名稱 = (string)reader["出版社編號名稱"];
                eb.b_Series_yn = (string)reader["b_Series_yn"];
                eb.b_ImagePath = "../書籍素材/" + eb.b_Type + "素材/" + eb.b_id + "/" + eb.b_id + "-cover.jpg";

                list.Add(eb);
            }

            reader.Close();
            con.Close();

            return list;
        }


        public List<string> 傳回出版社編號名稱()
        {
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
    }
}