using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeykentHospitalAppointment.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Branş Adı")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        public ICollection<Doctor>? Doctors { get; set; }
    }
}