using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ABCPharmacy.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        public string DoctorName { get; set; }

        public string Dept { get; set; }

        //Read - R
        public static List<Doctor> GetAllDoctors()
        {
            DataTable dt = DB.GetDataTable("Select * From Doctors");

            List<Doctor> lst = new List<Doctor>();

            foreach (DataRow item in dt.Rows)
            {
                lst.Add(
                    new Doctor()
                    {
                        DoctorId = (int)item["DoctorId"],
                        DoctorName = (string)item["DoctorName"],
                        Dept = (string)item["Dept"]
                    }
                    );
            }

            return lst;
        }


    }
}