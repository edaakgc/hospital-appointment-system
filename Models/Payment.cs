using System.ComponentModel.DataAnnotations;

namespace BeykentHospitalAppointment.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Display(Name = "Tutar")]
        public decimal Amount { get; set; }

        [Display(Name = "Ödeme Tipi")]
        public string PaymentType { get; set; } = string.Empty;

        [Display(Name = "Ödeme Durumu")]
        public string PaymentStatus { get; set; } = "Bekliyor";

        public int AppointmentId { get; set; }

        public Appointment? Appointment { get; set; }
    }
}