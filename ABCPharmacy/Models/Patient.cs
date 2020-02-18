using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ABCPharmacy.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        public string Firstname { get; set; }

        public string Surname { get; set; }

        public string Gender { get; set; }

        public DateTime DOB { get; set; }

        public string Address { get; set; }

        //Create - C
        public static int Create(string Firstname, string Surname, string Gender, DateTime DOB, string Address)
        {
            SqlConnection con = new SqlConnection(DB.constr);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "PatientInsert";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = con;


            cmd.Parameters.Add(new SqlParameter("firstname", Firstname));
            cmd.Parameters.Add(new SqlParameter("surname", Surname));
            cmd.Parameters.Add(new SqlParameter("gender", Gender));
            cmd.Parameters.Add(new SqlParameter("dob", DOB));
            cmd.Parameters.Add(new SqlParameter("address", Address));

            cmd.ExecuteNonQuery();


            cmd = new SqlCommand("select IDENT_CURRENT( 'patients' )");
            cmd.Connection = con;
            object id = cmd.ExecuteScalar();

            con.Close();

            return int.Parse(id.ToString());
        }

        //Read - R
        public static List<Patient> GetAllPatients()
        {
            DataTable dt = DB.GetDataTable("Select * From Patients");

            List<Patient> lst = new List<Patient>();

            foreach (DataRow item in dt.Rows)
            {
                lst.Add(
                    new Patient()
                    {
                        PatientId = (int)item["PatientId"],
                        Firstname = (string)item["Firstname"],
                        Surname = (string)item["Surname"],
                        Gender = (string)item["Gender"],
                        Address = (string)item["Address"],
                        DOB = (DateTime)item["DOB"]

                    }
                    );
            }

            return lst;
        }

        //Will return number of rows updated, will be 0 or 1
        //Update - U
        public static int Update(int PatientId, string Firstname, string Surname, string Gender, DateTime DOB, string Address)
        {
            SqlConnection con = new SqlConnection(DB.constr);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "PatientUpdate";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = con;

            cmd.Parameters.Add(new SqlParameter("patientid", PatientId));
            cmd.Parameters.Add(new SqlParameter("firstname", Firstname));
            cmd.Parameters.Add(new SqlParameter("surname", Surname));
            cmd.Parameters.Add(new SqlParameter("gender", Gender));
            cmd.Parameters.Add(new SqlParameter("dob", DOB));
            cmd.Parameters.Add(new SqlParameter("address", Address));

            int rowCount = cmd.ExecuteNonQuery();

            con.Close();

            return rowCount;
        }


        //Will return number of rows updated, will be 0 or 1
        //Delete - D
        public static int Delete(int PatientId)
        {
            SqlConnection con = new SqlConnection(DB.constr);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "PatientDelete";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = con;

            cmd.Parameters.Add(new SqlParameter("patientid", PatientId));

            int rowCount = cmd.ExecuteNonQuery();

            con.Close();

            return rowCount;
        }


    }

}