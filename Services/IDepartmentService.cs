using BeykentHospitalAppointment.Models;

namespace BeykentHospitalAppointment.Services
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllDepartmentsAsync();

        Task<Department?> GetDepartmentByIdAsync(int id);

        Task CreateDepartmentAsync(Department department);

        Task UpdateDepartmentAsync(Department department);

        Task<bool> DepartmentExistsAsync(int id);

        Task<bool> HasDoctorsAsync(int departmentId);

        Task<bool> DeleteDepartmentAsync(int id);
    }
}