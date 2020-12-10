using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentBook.Models.Test
{
    public class TestFactory
    {
        string myDBConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";

        public List<TestModel> ReturnAll()
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            string tSQL = "Select * from MemberAddDetail";
            SqlCommand cmd = new SqlCommand(tSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            List<TestModel> list = new List<TestModel>();

            while (reader.Read())
            {
                TestModel t = new TestModel();

                t.m_id = (string)reader["m_id"];
                t.Point = "+ " + ((int)reader["mad_AddPoint"]).ToString();
                t.Time = ((DateTime)reader["mad_AddTime"]).ToString();
                t.TotalPoint = ((int)reader["mad_TotalPoint"]).ToString();

                list.Add(t);
            }

            reader.Close();

            string tSQL1 = "Select * from MemberShopDetail";
            SqlCommand cmd1 = new SqlCommand(tSQL1, con);
            SqlDataReader reader1 = cmd1.ExecuteReader();

            while (reader1.Read())
            {
                TestModel t = new TestModel();

                t.m_id = (string)reader1["m_id"];
                t.b_id = (string)reader1["b_id"];
                t.Point = "-" + ((int)reader1["msd_CostPoint"]).ToString();
                t.Time = ((DateTime)reader1["msd_DateTime"]).ToString();
                t.TotalPoint = ((int)reader1["msd_TotalPoint"]).ToString();

                list.Add(t);
            }

            var newlist = (list.OrderBy(t => t.Time)).ToList();
            reader1.Close();

            con.Close();

            return newlist;
        }
    }
}