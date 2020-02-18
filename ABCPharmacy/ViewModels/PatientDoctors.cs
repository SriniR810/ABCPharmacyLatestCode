using ABCPharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABCPharmacy.ViewModels
{
    public class PatientDoctors
    {
        public Patient   Patient { get; set; }
        public List<Doctor> Doctors { get; set; }
    }
}