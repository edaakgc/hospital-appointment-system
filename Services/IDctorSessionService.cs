using BeykentHospitalAppointment.Models;

namespace BeykentHospitalAppointment.Services
{
    public interface IDoctorSessionService
    {
        Task<List<DoctorSession>> GetAllSessionsAsync();

        Task<List<DoctorSession>> GetAvailableSessionsByDoctorAsync(int doctorId);

        Task<DoctorSession?> GetSessionByIdAsync(int id);

        Task<DoctorSession?> GetSessionWithDoctorByIdAsync(int id);

        Task<DoctorSession?> GetAvailableSessionForBookingAsync(int sessionId);

        Task CreateSessionAsync(DoctorSession session);

        Task UpdateSessionAsync(DoctorSession session);

        Task<bool> SessionExistsAsync(int id);

        Task<bool> SameSessionExistsAsync(DoctorSession session, int? ignoredSessionId = null);

        Task<bool> HasAppointmentAsync(int sessionId);

        Task<bool> DeleteSessionAsync(int id);
    }
}