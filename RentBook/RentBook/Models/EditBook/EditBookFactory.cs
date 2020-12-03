﻿using System;
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

            string tSQL = "select A.b_Image,A.b_id,A.b_Name,A.b_Type,A.b_DatePrice,A.b_AgeRating,A.p_id + ' ' + B.p_Name as 出版社編號名稱,A.b_Series_yn From Books A left outer join Publishing B on A.p_id = B.p_id";

            SqlCommand cmd = new SqlCommand(tSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            List<EditBookModel> list = new List<EditBookModel>();

            while (reader.Read())
            {
                EditBookModel eb = new EditBookModel();

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

        public EditBookModel 帶出要修改的書籍資訊(string b_id)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "select A.b_Image,A.b_id,A.b_Name,A.b_Type,A.b_DatePrice,A.b_AgeRating,A.p_id + ' ' + B.p_Name as 出版社編號名稱,A.b_Series_yn From Books A left outer join Publishing B on A.p_id = B.p_id where A.b_id=@bid";

            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bid", b_id);
            SqlDataReader reader = cmd.ExecuteReader();

            EditBookModel eb = new EditBookModel();

            if (reader.Read())
            {
                eb.b_Image = (string)reader["b_Image"];
                eb.b_id = (string)reader["b_id"];
                eb.b_Name = (string)reader["b_Name"];
                eb.b_Type = (string)reader["b_Type"];
                eb.b_DatePrice = (int)reader["b_DatePrice"];
                eb.b_AgeRating = (string)reader["b_AgeRating"];
                eb.出版社編號名稱 = (string)reader["出版社編號名稱"];
                eb.b_Series_yn = (string)reader["b_Series_yn"];
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