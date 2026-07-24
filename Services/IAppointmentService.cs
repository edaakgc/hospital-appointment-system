using BeykentHospitalAppointment.Models;

namespace BeykentHospitalAppointment.Services
{
    public interface IAppointmentService
    {
        Task<List<Appointment>> GetAllAppointmentsAsync();

        Task<List<Appointment>> GetAppointmentsByPatientAsync(int patientId);

        Task<bool> CreateAppointmentAsync(int patientId, int sessionId, string paymentType);

        Task<bool> CancelAppointmentAsync(int appointmentId, int patientId);
    }
}