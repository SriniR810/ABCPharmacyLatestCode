using ABCPharmacy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ABCPharmacy.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Summary()
        {
            return View();
        }

        public string Appointment(string dateFrom, string dateTo)
        {
            DataTable dt = DB.GetDataTable($"Select A.AppointmentId, A.PatientId, P.Firstname, P.Surname, P.Gender, P.DOB, A.DoctorId, D.DoctorName, A.Fees, A.Symptoms, A.DOA, A.Indications, A.Medications,A.Comments, A.Advice From Appointments A, Patients P, Doctors D Where A.DOA Between '{dateFrom}' And '{dateTo}' And A.PatientId = P.PatientId And A.DoctorId = D.DoctorId");

            List<object> lst = new List<object>();

            foreach (DataRow item in dt.Rows)
            {
                lst.Add(
                    new //annonimous object
                    {
                        AppointmentId = (int)item["AppointmentId"],
                        PatientId = (int)item["PatientId"],
                        PatientName = item["Firstname"] + " " + item["Surname"],
                        Age = ((DateTime.Now - (DateTime)item["DOB"]).Days) / 365,
                        Gender = (string)item["Gender"],
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
                    );
            }

            //var arr = lst.ToArray();
            //var json = new JavaScriptSerializer().Serialize(arr);

            var json = new JavaScriptSerializer().Serialize(lst);
            json = "{\"data\":" + json + "}";

            return json;
        }

        //service method (web method/web api)
        public JsonResult AppointmentSummary()
        {
            string dateFrom = Request["dateFrom"];
            string dateTo = Request["dateTo"];

            DataTable dt = DB.GetDataTable($"Select A.AppointmentId, A.PatientId, P.Firstname, P.Surname, P.Gender, P.DOB, A.DoctorId, D.DoctorName, A.Fees, A.Symptoms, A.DOA, A.Indications, A.Medications,A.Comments, A.Advice From Appointments A, Patients P, Doctors D Where A.DOA Between '{dateFrom}' And '{dateTo}' And A.PatientId = P.PatientId And A.DoctorId = D.DoctorId");

            string msg = "";

            double totalFees = 0;


            foreach (DataRow item in dt.Rows)
            {
                totalFees += (double)item["Fees"];
            }

            msg = "Total Fees: " + totalFees.ToString("c") + "<br/>Total Consultations: " + dt.Rows.Count;


            dt.Columns.Add("Age", typeof(int));

            foreach (DataRow item in dt.Rows)
            {
                item["Age"] = ((DateTime.Now - (DateTime)item["DOB"]).Days) / 365;
            }

            //define 3 age groups: 0 to 10, 10 to 21, 21 above

            //0-10
            int numOfConsultationsBelow10 = 0;
            double feesCollectedBelow10 = 0;

            int numOfConsultationsBelow22 = 0;
            double feesCollectedBelow22 = 0;


            int numOfConsultations22Above = 0;
            double feesCollected22Above = 0;


            foreach (DataRow item in dt.Rows)
            {
                int age = (int)item["Age"];
                if (age < 10)//0-9
                {
                    numOfConsultationsBelow10 += 1;
                    feesCollectedBelow10 += (double)item["Fees"];
                }
                else if (age < 22)//10 to 21
                {
                    numOfConsultationsBelow22 += 1;
                    feesCollectedBelow22 += (double)item["Fees"];
                }
                else
                {
                    numOfConsultations22Above += 1;
                    feesCollected22Above += (double)item["Fees"];

                }

            }
            msg += "<br/>";
            msg += $"<table border='1'><tr><th>Age Groups</th><th>0-9</th>&nbsp;&nbsp;&nbsp;&nbsp;<th>10-21</th>&nbsp;&nbsp;&nbsp;&nbsp;<th>21 Above</th></tr><tr><th>Fees</th><th>{feesCollectedBelow10}</th><th>{feesCollectedBelow22}</th><th>{feesCollected22Above}</th></tr><tr><th>Consultations</th><th>{numOfConsultationsBelow10}</th><th>{numOfConsultationsBelow22}</th><th>{numOfConsultations22Above}</th></tr></table>";
            
            return Json(new { Status = "Success", Msg = msg });


        }

    }
}