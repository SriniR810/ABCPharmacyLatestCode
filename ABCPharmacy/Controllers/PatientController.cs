using ABCPharmacy.Models;
using ABCPharmacy.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABCPharmacy.Controllers
{
    public class PatientController : Controller
    {

        public ActionResult Index()
        {
            var lst = Patient.GetAllPatients();
            return View(lst);
        }
        // GET: Patient
        [HttpGet]
        public ActionResult Create()
        {
            Patient patient = new Patient();

            return View(patient);
        }

        [HttpPost]
        public ActionResult Create(Patient model)
        {
            Patient.Create(model.Firstname, model.Surname, model.Gender, model.DOB, model.Address);

            return RedirectToAction("Index");
        }


        // GET: Patient
        [HttpGet]
        public ActionResult Edit(int id)//receive from the link
        {
            Patient patient = Patient.GetAllPatients().Find(p => p.PatientId == id);

            return View(patient);
        }

        [HttpPost]
        public ActionResult Edit(Patient model)
        {
            Patient.Update(model.PatientId, model.Firstname, model.Surname, model.Gender, model.DOB, model.Address);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Appointment(int id)
        {
            Patient patient = Patient.GetAllPatients().Find(p => p.PatientId == id);

            List<Doctor> doctors = Doctor.GetAllDoctors();

            PatientDoctors patientDoctors = new PatientDoctors()
            {
                Patient = patient,
                Doctors = doctors
            };

            return View(patientDoctors);
        }

        [HttpPost]
        [ActionName("Appointment")]
        public ActionResult AppointmentPost()
        {
            
            int appointmentId = ABCPharmacy.Models.Appointment.CreateAppointment(int.Parse(Request["PatientId"]), int.Parse(Request["DoctorId"]), DateTime.Parse(Request["DOA"]), Request["Symptoms"], Request["Indications"], Request["Advice"], Request["Medications"], Request["Comments"], double.Parse(Request["Fees"]));

            TempData["Msg"] = "New Appointment Successfully created, with Appointment Id = " + appointmentId;

            return RedirectToAction("Appointments");
        }


        public ActionResult Appointments()
        {
            if(TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"];
            }

            var lst = ABCPharmacy.Models.Appointment.GetAppointments();
            return View(lst);
        }

        [HttpGet]
        public ActionResult EditAppointment(int id)
        {
            Appointment appointment = ABCPharmacy.Models.Appointment.GetAppointments().Find(a => a.AppointmentId == id);

            return View(appointment);
        }

        [HttpPost]
        [ActionName("EditAppointment")]
        public ActionResult EditAppointment_Post()
        {
            Appointment appointment = ABCPharmacy.Models.Appointment.GetAppointments().Find(a => a.AppointmentId == int.Parse(Request["AppointmentId"]));

            //let the doctor edit only these 4 fields...
            appointment.Indications = Request["Indications"];
            appointment.Comments = Request["Comments"];
            appointment.Advice = Request["Advice"];
            appointment.Fees = double.Parse(Request["Fees"]);
            appointment.Medications = Request["Medications"];

            ABCPharmacy.Models.Appointment.UpdateAppointment(appointment);

            ViewBag.Msg = "Appointment Updated!";

            return RedirectToAction("Appointments");
        }

        public ActionResult Print(int id)
        {            

            var lst = ABCPharmacy.Models.Appointment.GetAppointments();
            var appointment = lst.Find(a => a.AppointmentId == id);
                       

            return View(appointment);
        }

     //delete patient
        public ActionResult Delete(int id)
        {
            Patient.Delete(id);

            return RedirectToAction("Index");
        }



        //delete patient
        public ActionResult DeleteAppointment(int id)
        {
            ABCPharmacy.Models.Appointment.DeleteAppointment(id);

            return RedirectToAction("Appointments");
        }
    }
}