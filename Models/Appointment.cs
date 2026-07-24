using System;
using System.ComponentModel.DataAnnotations;

namespace BeykentHospitalAppointment.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Display(Name = "Randevu Tarihi")]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "Randevu Durumu")]
        public string Status { get; set; } = "Aktif";

        public int PatientId { get; set; }

        public Patient? Patient { get; set; }

        public int DoctorId { get; set; }

        public Doctor? Doctor { get; set; }

        public int DoctorSessionId { get; set; }

        public DoctorSession? DoctorSession { get; set; }

        public Payment? Payment { get; set; }
    }
}