namespace BeykentHospitalAppointment.Services
{
    public interface IAdminDashboardService
    {
        Task<int> GetDepartmentCountAsync();

        Task<int> GetDoctorCountAsync();

        Task<int> GetSessionCountAsync();

        Task<int> GetAppointmentCountAsync();
    }
}