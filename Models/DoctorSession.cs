using System;
using System.ComponentModel.DataAnnotations;

namespace BeykentHospitalAppointment.Models
{
    public class DoctorSession
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Seans Tarihi")]
        public DateTime SessionDate { get; set; }

        [Required]
        [Display(Name = "Başlangıç Saati")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Display(Name = "Bitiş Saati")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "Müsait mi?")]
        public bool IsAvailable { get; set; } = true;

        public int DoctorId { get; set; }

        public Doctor? Doctor { get; set; }

        public Appointment? Appointment { get; set; }
    }
}