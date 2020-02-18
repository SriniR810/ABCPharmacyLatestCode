using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ABCPharmacy.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; }

        public int DoctorId { get; set; }

        public string DoctorName { get; set; }

        public string DOA { get; set; }

        public string Symptoms { get; set; }

        public string Indications { get; set; }

        public string Advice { get; set; }

        public string Medications { get; set; }

        public string Comments { get; set; }

        public double Fees { get; set; }


        public static int CreateAppointment(int PatientId, int DoctorId, DateTime DOA, string Symptoms, string Indications, string Advice, string Medications, string Comments, double Fees)
        {
            SqlConnection con = new SqlConnection(DB.constr);
            con.Open();

            SqlCommand cmd = new SqlCommand("Insert Into Appointments (PatientId, DoctorId, DOA, Symptoms, Indications,Advice,Medications,Comments,Fees) values (@PatientId, @DoctorId, @DOA, @Symptoms, @Indications,@Advice,@Medications,@Comments, @Fees)", con);

            cmd.Parameters.Add(new SqlParameter("PatientId", PatientId));
            cmd.Parameters.Add(new SqlParameter("DoctorId", DoctorId));
            cmd.Parameters.Add(new SqlParameter("DOA", DOA));
            cmd.Parameters.Add(new SqlParameter("Symptoms", Symptoms));
            cmd.Parameters.Add(new SqlParameter("Indications", Indications));
            cmd.Parameters.Add(new SqlParameter("Advice", Advice));
            cmd.Parameters.Add(new SqlParameter("Medications", Medications));
            cmd.Parameters.Add(new SqlParameter("Comments", Comments));
            cmd.Parameters.Add(new SqlParameter("Fees", Fees));

            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("select IDENT_CURRENT( 'Appointments' )");
            cmd.Connection = con;
            object id = cmd.ExecuteScalar();


            con.Close();

            return int.Parse(id.ToString());
        }

        public static List<Appointment> GetAppointments()
        {

            DataTable dt = DB.GetDataTable($"Select A.AppointmentId, A.PatientId, P.Firstname, P.Surname, P.Gender, P.DOB, A.DoctorId, D.DoctorName, A.Fees, A.Symptoms, A.DOA, A.Indications, A.Medications,A.Comments, A.Advice From Appointments A, Patients P, Doctors D Where A.PatientId = P.PatientId And A.DoctorId = D.DoctorId");

            List<Appointment> lst = new List<Appointment>();

            foreach (DataRow item in dt.Rows)
            {
                lst.Add(
                    new Appointment()
                    {
                        AppointmentId = (int)item["AppointmentId"],
                        PatientId = (int)item["PatientId"],
                        PatientName = (string)item["Firstname"] + " " + item["Surname"],
                        DoctorId = (int)item["DoctorId"],
                        DoctorName = (string)item["DoctorName"],
                        Fees = (double)item["Fees"],
                        Symptoms = (string)item["Symptoms"],
                        DOA = ((DateTime)item["DOA"]).ToString("dd-MMM-yyyy"),
                        Indications = (string)item["Indications"],
                        Medications = (string)item["Medications"],
                        Advice = (string)item["Advice"],
                        Comments = (string)item["Comments"],
                    }
                    ) ;
            }

            return lst;
        }

        public static int UpdateAppointment(Appointment appointment)
        {
            SqlConnection con = new SqlConnection(DB.constr);
            con.Open();

            SqlCommand cmd = new SqlCommand("Update Appointments SET Indications=@Indications,Advice=@Advice,Medications=@Medications,Comments=@Comments,Fees=@Fees  Where AppointmentId=@AppointmentId", con);

            //cmd.Parameters.Add(new SqlParameter("PatientId", PatientId));
            //cmd.Parameters.Add(new SqlParameter("DoctorId", DoctorId));
            //cmd.Parameters.Add(new SqlParameter("DOA", DOA));
            //cmd.Parameters.Add(new SqlParameter("Symptoms", Symptoms));
            cmd.Parameters.Add(new SqlParameter("Indications", appointment.Indications));
            cmd.Parameters.Add(new SqlParameter("Advice", appointment.Advice));
            cmd.Parameters.Add(new SqlParameter("Medications", appointment.Medications));
            cmd.Parameters.Add(new SqlParameter("Comments", appointment.Comments));
            cmd.Parameters.Add(new SqlParameter("Fees", appointment.Fees));
            cmd.Parameters.Add(new SqlParameter("AppointmentId", appointment.AppointmentId));

            int x = cmd.ExecuteNonQuery();


            con.Close();

            return x;
        }



        public static int DeleteAppointment(int AppointmentId)
        {
            SqlConnection con = new SqlConnection(DB.constr);
            con.Open();

            SqlCommand cmd = new SqlCommand("Delete From Appointments  Where AppointmentId=@AppointmentId", con);

            cmd.Parameters.Add(new SqlParameter("AppointmentId", AppointmentId));

            int x = cmd.ExecuteNonQuery();


            con.Close();

            return x;
        }
    }
}