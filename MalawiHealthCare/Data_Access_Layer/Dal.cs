using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MalawiHealthCare.Data_Access_Layer
{
    public class Dal
    {
        SqlConnection con;
        DataTable dt = new DataTable();

        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        public string Authorizer = "Normal User";
        public Dal()
        {
            //con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings.Get("connection"));

            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString);

        }
        public void ClearDataTable()
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dt.Rows.Clear();
                    dt.Columns.Clear();
                    dt.Clear();
                }
            }
        }

        public DataTable Fun_MenuItems(int UserId)
        {

            ClearDataTable();
            da = new SqlDataAdapter("Exec MenuItems_P '" + UserId + "'", con);
            da.SelectCommand.CommandTimeout = 300;
            ds.Clear();
            da.Fill(ds, "Menus_Users");
            dt = ds.Tables["Menus_Users"];
            return dt;
        }

        public DataTable Fun_AccessRights(int UserId, int MenuId, string Tran)
        {

            ClearDataTable();
            da = new SqlDataAdapter("Exec FormAccess_P '" + UserId + "','" + MenuId + "','" + Tran + "'", con);
            da.SelectCommand.CommandTimeout = 300;
            ds.Clear();
            da.Fill(ds, "Menus_Users");
            dt = ds.Tables["Menus_Users"];
            return dt;
        }


    }
}