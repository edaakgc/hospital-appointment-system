using BeykentHospitalAppointment.Models;

namespace BeykentHospitalAppointment.Services
{
    public interface IDoctorService
    {
        Task<List<Doctor>> GetAllDoctorsAsync();

        Task<List<Doctor>> GetDoctorsByDepartmentAsync(int departmentId);

        Task<Doctor?> GetDoctorByIdAsync(int id);

        Task<Doctor?> GetDoctorWithDepartmentByIdAsync(int id);

        Task CreateDoctorAsync(Doctor doctor);

        Task UpdateDoctorAsync(Doctor doctor);

        Task<bool> DoctorExistsAsync(int id);

        Task<bool> HasSessionsAsync(int doctorId);

        Task<bool> HasAppointmentsAsync(int doctorId);

        Task<bool> DeleteDoctorAsync(int id);
    }
}