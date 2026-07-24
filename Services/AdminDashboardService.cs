using BeykentHospitalAppointment.Data;
using Microsoft.EntityFrameworkCore;

namespace BeykentHospitalAppointment.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetDepartmentCountAsync()
        {
            return await _context.Departments.CountAsync();
        }

        public async Task<int> GetDoctorCountAsync()
        {
            return await _context.Doctors.CountAsync();
        }

        public async Task<int> GetSessionCountAsync()
        {
            return await _context.DoctorSessions.CountAsync();
        }

        public async Task<int> GetAppointmentCountAsync()
        {
            return await _context.Appointments.CountAsync();
        }
    }
}