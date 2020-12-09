using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentBook.Models
{
    public class CMemberFactory
    {
        public CMember authenticated(string email, string password)
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("S_ID", (object)email));
            paras.Add(new SqlParameter("S_PWD", (object)password));

            List<CMember> list = getBySql(
                //"SELECT * FROM SystemAccount WHERE s_id=@S_ID AND s_Pwd=@S_PWD",
                "SELECT B.* From SystemAccount A join Member B on A.s_id = B.m_Email Where A.s_id =@S_ID AND A.s_Pwd=@S_PWD",
                paras);

            if (list.Count == 0)
                return null;
            return list[0];

        }
        private void executeSql(string sql, List<SqlParameter> paras)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            if (paras != null)
            {
                foreach (SqlParameter p in paras)
                    cmd.Parameters.Add(p);
            }
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void delete(CMember p)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("M_ID", (object)p.m_id));

            executeSql("DELETE FROM tCustomer WHERE m_id=@M_ID", list);
        }

        public List<CMember> getAll()
        {
            return getBySql("SELECT * FROM Member ", null);
        }

        
        public CMember getById(int fId)
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("M_ID", (object)fId));

            List<CMember> list = getBySql("SELECT * FROM Member WHERE m_id=@M_ID", paras);
            if (list.Count == 0)
                return null;
            return list[0];

        }

        public string get_m_id()
        {
            string s_Max_m_id = "";
            string sql = "Select Max(m_id) as Max_m_id";
            sql = sql + "   From Member";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            SqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                if (reader["Max_m_id"].ToString() == "")
                {
                    s_Max_m_id = "M00001";
                }
                else
                {
                    s_Max_m_id = reader["Max_m_id"].ToString();
                    string s_temp = s_Max_m_id.Substring(1);
                    int intId = 0;
                    Int32.TryParse(s_temp, out intId);
                    intId++;
                    s_Max_m_id = "M" + String.Format("{0:00000}", intId);
                }
                
            }

            con.Close();
            return s_Max_m_id;
        }

        private List<CMember> getBySql(string sql, List<SqlParameter> paras)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            if (paras != null)
            {
                foreach (SqlParameter p in paras)
                    cmd.Parameters.Add(p);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            List<CMember> list = new List<CMember>();
            while (reader.Read())
            {
                CMember x = new CMember();
                //x.fId = (int)reader["fId"]; ;
                //x.fName = reader["fName"].ToString();
                //x.fAddress = reader["fAddress"].ToString();
                //x.fEmail = reader["fEmail"].ToString();
                //x.fPassword = reader["fPassword"].ToString();
                //x.fPhone = reader["fPhone"].ToString();
                //x.fLevel = (int)reader["fLevel"];
                x.m_id = reader["m_id"].ToString();
                x.m_Name = reader["m_Name"].ToString();
                x.m_Birth = (DateTime)reader["m_Birth"];
                x.m_Gender = reader["m_Gender"].ToString();
                //x.m_Point = (int)reader["m_Point"];
                x.m_Email = reader["m_Email"].ToString();
                x.bc_id = (int)reader["bc_id"];
                //x.m_Image = reader["m_Image"].ToString();
                //x.m_RegisterDate = (DateTime)reader["m_RegisterDate"];
                //x.m_LastLogin = (DateTime)reader["m_LastLogin"];
                //x.m_LastLogon = (DateTime)reader["m_LastLogon"];
                //x.m_OnlineTime = (DateTime)reader["m_OnlineTime"];
                //x.m_MonthlyLastTime = (DateTime)reader["m_MonthlyLastTime"];//
                //x.bc_id = (int)reader["bc_id"];
                list.Add(x);
            }
            con.Close();
            return list;
        }

        public void update(CMember p)
        {
            string sql = "UPDATE Member SET ";
            sql += "m_Name=@M_NAME,";
            sql += "m_Birth=@M_BIRTH,";
            sql += "m_Gender=@M_GENDER,";
            sql += "m_Point=@M_POINT,";
            //if (!string.IsNullOrEmpty(p.fAddress))
            //    sql += "fAddress=@FADDRESS,";
            sql += "m_Email=@M_EMAIL,";
            sql += "m_Image=@M_IMAGE,";
            sql += "m_RegisterDate=@M_REGISTERDATE,";
            sql += "m_LastLogin=@M_LASTLOGIN,";
            sql += "m_LastLogon=@M_LASTLOGON,";
            sql += "m_OnlineTime=@M_ONLINETIME,";
            sql += "m_MonthlyLastTime=@M_MONTHLYLASTTIME,";
            sql += "bc_id=@BC_ID,";
            sql += " WHERE m_id=@M_ID";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("M_ID", (object)p.m_id));
            paras.Add(new SqlParameter("M_NAME", (object)p.m_Name));
            paras.Add(new SqlParameter("M_BIRTH", (object)p.m_Birth));
            paras.Add(new SqlParameter("M_GENDER", (object)p.m_Gender));
            //if (!string.IsNullOrEmpty(p.fAddress))
            //    paras.Add(new SqlParameter("FADDRESS", p.fAddress));
            paras.Add(new SqlParameter("M_POINT", (object)p.m_Point));
            paras.Add(new SqlParameter("M_EMAIL", (object)p.m_Email));

            paras.Add(new SqlParameter("M_IMAGE", (object)p.m_Image));
            paras.Add(new SqlParameter("M_REGISTERDATE", (object)p.m_RegisterDate));
            paras.Add(new SqlParameter("M_LASTLOGIN", (object)p.m_LastLogin));
            paras.Add(new SqlParameter("M_LASTLOGON", (object)p.m_LastLogon));
            paras.Add(new SqlParameter("M_ONLINETIME", (object)p.m_OnlineTime));
            paras.Add(new SqlParameter("M_MONTHLYLASTTIME", (object)p.m_MonthlyLastTime));
            paras.Add(new SqlParameter("BC_ID", (object)p.bc_id));
            executeSql(sql, paras);

        }

        public void create(CMember p, string passWord)
        {
            //string sql = "INSERT INTO Member(";
            //sql += "m_id,";
            //sql += "m_Name,";
            //sql += "m_Birth,";
            ////if (!string.IsNullOrEmpty(p.fAddress))
            ////    sql += "fAddress,";
            //sql += "m_Gender,";
            //sql += "m_Point,";
            //sql += "m_Email,";
            //sql += "m_Image,";
            //sql += "m_RegisterDate,";
            //sql += "m_LastLogin,";
            //sql += "m_LastLogon,";
            //sql += "m_OnlineTime,";
            //sql += "m_MonthlyLastTime,";
            //sql += "bc_id,";
            //sql += ")VALUES(";
            //sql += "@M_ID,";
            //sql += "@M_NAME,";
            //sql += "@M_BIRTH,";
            ////if (!string.IsNullOrEmpty(p.fAddress))
            ////    sql += "@FADDRESS,";
            //sql += "@M_GENDER,";
            //sql += "@M_POINT,";
            //sql += "@M_EMAIL,";
            //sql += "@M_IMAGE,";
            //sql += "@M_REGISTERDATE,";
            //sql += "@M_LASTLOGIN,";
            //sql += "@M_LASTLOGON,";
            //sql += "@M_ONLINETIME,";
            //sql += "@M_MONTHLYLASTTIME,";
            //sql += "@BC_ID)";
            //List<SqlParameter> paras = new List<SqlParameter>();
            //paras.Add(new SqlParameter("M_ID", (object)p.m_id));
            //paras.Add(new SqlParameter("M_NAME", (object)p.m_Name));
            //paras.Add(new SqlParameter("M_BIRTH", (object)p.m_Birth));
            ////if (!string.IsNullOrEmpty(p.fAddress))
            ////    paras.Add(new SqlParameter("FADDRESS", p.fAddress));
            //paras.Add(new SqlParameter("M_GENDER", (object)p.m_Gender));
            //paras.Add(new SqlParameter("M_POINT", (object)p.m_Point));

            //paras.Add(new SqlParameter("M_EMAIL", (object)p.m_Email));
            //paras.Add(new SqlParameter("M_IMAGE", (object)p.m_Image));
            //paras.Add(new SqlParameter("M_REGISTERDATE", (object)p.m_RegisterDate));
            //paras.Add(new SqlParameter("M_LASTLOGIN", (object)p.m_LastLogin));
            //paras.Add(new SqlParameter("M_LASTLOGON", (object)p.m_LastLogon));
            //paras.Add(new SqlParameter("M_ONLINETIME", (object)p.m_OnlineTime));
            //paras.Add(new SqlParameter("M_MONTHLYLASTTIME", (object)p.m_MonthlyLastTime));
            //paras.Add(new SqlParameter("BC_ID", (object)p.bc_id));
            //executeSql(sql, paras);

            string sql = "INSERT INTO Member(";
            sql += "m_id,";
            sql += "m_Name,";
            sql += "m_Alias,";
            sql += "m_Birth,";
            sql += "m_Gender,";
            sql += "m_Email,";
            sql += "m_Intro,";
            sql += "m_Image";
            sql += ")VALUES(";
            sql += "@M_ID,";
            sql += "@M_NAME,";
            sql += "@M_NAME,";
            sql += "@M_BIRTH,";
            sql += "@M_GENDER,";
            sql += "@M_EMAIL,";
            sql += "'這人很懶，什麼都沒有留下',";
            sql += "'Default.jpg')";//
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("M_ID", (object)p.m_id));
            paras.Add(new SqlParameter("M_NAME", (object)p.m_Name));
            paras.Add(new SqlParameter("M_BIRTH", (object)p.m_Birth));
            paras.Add(new SqlParameter("M_GENDER", (object)p.m_Gender));
            paras.Add(new SqlParameter("M_EMAIL", (object)p.m_Email));
            executeSql(sql, paras);

            sql = "";
            sql = "INSERT INTO SystemAccount(";
            sql += "s_id,";
            sql += "s_Pwd,";
            sql += "r_id";
            sql += ")VALUES(";
            sql += "@S_ID,";
            sql += "@S_PWD,";
            sql += "@R_ID)";
            List<SqlParameter> paras_1 = new List<SqlParameter>();
            string s_R_ID = "1";
            paras_1.Add(new SqlParameter("S_ID", (object)p.m_Email));
            paras_1.Add(new SqlParameter("S_PWD", (object)passWord));
            paras_1.Add(new SqlParameter("R_ID", (object)s_R_ID));
            executeSql(sql, paras_1);

            //1091208 寫入bc_id
            sql = "";
            sql = "Insert Into BookCase (bc_Name)";
            sql += "  Values (@BC_NAME) ";
            List<SqlParameter> paras_1_1 = new List<SqlParameter>();
            paras_1_1.Add(new SqlParameter("BC_NAME", (object)p.m_id));
            executeSql(sql, paras_1_1);

            string s_BC_ID = "1";
            s_BC_ID = get_bc_id();

            sql = "";
            sql = "Update Member";
            sql += "  Set bc_id = @BC_ID ";
            sql += " Where m_id = @M_ID";
            List<SqlParameter> paras_2 = new List<SqlParameter>();
            paras_2.Add(new SqlParameter("BC_ID", (object)s_BC_ID));
            paras_2.Add(new SqlParameter("M_ID", (object)p.m_id));
            executeSql(sql, paras_2);

            
        }

        public string get_bc_id()
        {
            string s_Max_bc_id = "";
            string sql = "Select Max(bc_id) as Max_bc_id";
            sql = sql + "   From BookCase";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=RentBookdb;Integrated Security=True";
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                s_Max_bc_id = reader["Max_bc_id"].ToString();

            }

            con.Close();
            return s_Max_bc_id;
        }
    }
}