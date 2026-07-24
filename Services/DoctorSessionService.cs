using BeykentHospitalAppointment.Data;
using BeykentHospitalAppointment.Models;
using Microsoft.EntityFrameworkCore;

namespace BeykentHospitalAppointment.Services
{
    public class DoctorSessionService : IDoctorSessionService
    {
        private readonly ApplicationDbContext _context;

        public DoctorSessionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DoctorSession>> GetAllSessionsAsync()
        {
            return await _context.DoctorSessions
                .Include(s => s.Doctor)
                    .ThenInclude(d => d!.Department)
                .OrderBy(s => s.SessionDate)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<List<DoctorSession>> GetAvailableSessionsByDoctorAsync(int doctorId)
        {
            return await _context.DoctorSessions
                .Where(s => s.DoctorId == doctorId && s.IsAvailable)
                .OrderBy(s => s.SessionDate)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<DoctorSession?> GetSessionByIdAsync(int id)
        {
            return await _context.DoctorSessions
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<DoctorSession?> GetSessionWithDoctorByIdAsync(int id)
        {
            return await _context.DoctorSessions
                .Include(s => s.Doctor)
                    .ThenInclude(d => d!.Department)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<DoctorSession?> GetAvailableSessionForBookingAsync(int sessionId)
        {
            return await _context.DoctorSessions
                .Include(s => s.Doctor)
                    .ThenInclude(d => d!.Department)
                .FirstOrDefaultAsync(s => s.Id == sessionId && s.IsAvailable);
        }

        public async Task CreateSessionAsync(DoctorSession session)
        {
            _context.DoctorSessions.Add(session);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSessionAsync(DoctorSession session)
        {
            _context.DoctorSessions.Update(session);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> SessionExistsAsync(int id)
        {
            return await _context.DoctorSessions
                .AnyAsync(s => s.Id == id);
        }

        public async Task<bool> SameSessionExistsAsync(DoctorSession session, int? ignoredSessionId = null)
        {
            return await _context.DoctorSessions.AnyAsync(s =>
                s.Id != ignoredSessionId &&
                s.DoctorId == session.DoctorId &&
                s.SessionDate.Date == session.SessionDate.Date &&
                s.StartTime == session.StartTime);
        }

        public async Task<bool> HasAppointmentAsync(int sessionId)
        {
            return await _context.Appointments
                .AnyAsync(a => a.DoctorSessionId == sessionId);
        }

        public async Task<bool> DeleteSessionAsync(int id)
        {
            var session = await _context.DoctorSessions.FindAsync(id);

            if (session == null)
            {
                return false;
            }

            bool hasAppointment = await HasAppointmentAsync(id);

            if (hasAppointment)
            {
                return false;
            }

            _context.DoctorSessions.Remove(session);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}