using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ABCPharmacy
{
    //DAL class
    public class DB
    {
        public static string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        //public static void ExecuteCommand(string )

        public static DataTable GetDataTable(string query)
        {
            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter(query, DB.constr);

            da.Fill(dt);

            return dt;
        }

    }
}