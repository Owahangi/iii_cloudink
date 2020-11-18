using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentBook.Models
{
    public class AddBookFactory
    {
        string myDBConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";

        // 新增書籍基本資料
        // 使用資料表：Books、BooksAuthor
        public void Create(Books b,BooksAuthor ba)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            //Insert into Books (b_id,b_Name,b_Info,b_Image,b_Type,b_PublishedDate,b_HourPrice,b_ISBN,b_AgeRating,p_id)Values(1234,'aaa','bbb','c','d','1996/09/07','1','g',1,1)

            string tSQL = "Insert into Books (b_id,b_Name,b_Info,b_Image,b_Type,b_PublishedDate,b_HourPrice,b_ISBN,b_AgeRating,p_id)Values(" +
                "@bid,@bName,@bInfo,@bImage,@bType,@bPublishedDate,@bHourPrice,@bISBN,@bAgeRating,@pID)";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            cmd.Parameters.AddWithValue("bid", "1236");
            cmd.Parameters.AddWithValue("bName", b.b_Name);
            cmd.Parameters.AddWithValue("bInfo", b.b_Info);
            cmd.Parameters.AddWithValue("bImage", b.b_Image);
            cmd.Parameters.AddWithValue("bType", b.b_Type);
            cmd.Parameters.AddWithValue("bPublishedDate", b.b_PublishedDate);
            cmd.Parameters.AddWithValue("bHourPrice", b.b_HourPrice);
            cmd.Parameters.AddWithValue("bISBN", b.b_ISBN);
            cmd.Parameters.AddWithValue("bAgeRating", b.b_AgeRating);
            cmd.Parameters.AddWithValue("pID", b.p_id);

            cmd.ExecuteNonQuery();

            string tSQL1 = "Insert into BooksAuthor (b_id,a_id)Values(@bid,@aid)";
            SqlCommand cmd1 = new SqlCommand(tSQL1, con);
            cmd1.Parameters.AddWithValue("bid", "1236");
            cmd1.Parameters.AddWithValue("aid", ba.a_id);

            cmd1.ExecuteNonQuery();
            con.Close();
        }
    }
}