using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeykentHospitalAppointment.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Hasta Adı Soyadı")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "TC Kimlik Numarası")]
        public string? IdentityNumber { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}