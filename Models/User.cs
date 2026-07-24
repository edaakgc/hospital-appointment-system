using System.ComponentModel.DataAnnotations;

namespace BeykentHospitalAppointment.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Rol")]
        public string Role { get; set; } = "Patient";

        public Patient? Patient { get; set; }
    }
}