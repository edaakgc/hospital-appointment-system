using BeykentHospitalAppointment.Data;
using BeykentHospitalAppointment.Models;
using Microsoft.EntityFrameworkCore;

namespace BeykentHospitalAppointment.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDbContext _context;

        public DoctorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            return await _context.Doctors
                .Include(d => d.Department)
                .OrderBy(d => d.FullName)
                .ToListAsync();
        }

        public async Task<List<Doctor>> GetDoctorsByDepartmentAsync(int departmentId)
        {
            return await _context.Doctors
                .Where(d => d.DepartmentId == departmentId)
                .OrderBy(d => d.FullName)
                .ToListAsync();
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            return await _context.Doctors
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Doctor?> GetDoctorWithDepartmentByIdAsync(int id)
        {
            return await _context.Doctors
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task CreateDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoctorExistsAsync(int id)
        {
            return await _context.Doctors
                .AnyAsync(d => d.Id == id);
        }

        public async Task<bool> HasSessionsAsync(int doctorId)
        {
            return await _context.DoctorSessions
                .AnyAsync(s => s.DoctorId == doctorId);
        }

        public async Task<bool> HasAppointmentsAsync(int doctorId)
        {
            return await _context.Appointments
                .AnyAsync(a => a.DoctorId == doctorId);
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return false;
            }

            bool hasSessions = await HasSessionsAsync(id);
            bool hasAppointments = await HasAppointmentsAsync(id);

            if (hasSessions || hasAppointments)
            {
                return false;
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}