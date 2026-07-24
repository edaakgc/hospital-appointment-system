using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeykentHospitalAppointment.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Doktor Adı Soyadı")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Uzmanlık Alanı")]
        public string Specialty { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Muayene Ücreti")]
        public decimal ExaminationFee { get; set; }

        public int DepartmentId { get; set; }

        public Department? Department { get; set; }

        public ICollection<DoctorSession>? DoctorSessions { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}