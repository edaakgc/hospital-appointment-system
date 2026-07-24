using BeykentHospitalAppointment.Data;
using BeykentHospitalAppointment.Models;
using Microsoft.EntityFrameworkCore;

namespace BeykentHospitalAppointment.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.Department)
                .Include(a => a.DoctorSession)
                .Include(a => a.Payment)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByPatientAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.Department)
                .Include(a => a.DoctorSession)
                .Include(a => a.Payment)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<bool> CreateAppointmentAsync(int patientId, int sessionId, string paymentType)
        {
            var session = await _context.DoctorSessions
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(s => s.Id == sessionId && s.IsAvailable);

            if (session == null || session.Doctor == null)
            {
                return false;
            }

            var appointmentDate = session.SessionDate.Date + session.StartTime;

            var appointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = session.DoctorId,
                DoctorSessionId = session.Id,
                AppointmentDate = appointmentDate,
                Status = "Aktif"
            };

            _context.Appointments.Add(appointment);

            session.IsAvailable = false;
            _context.DoctorSessions.Update(session);

            await _context.SaveChangesAsync();

            var payment = new Payment
            {
                AppointmentId = appointment.Id,
                Amount = session.Doctor.ExaminationFee,
                PaymentType = paymentType,
                PaymentStatus = paymentType == "Online Ödeme"
                    ? "Ödendi"
                    : "Hastanede Ödenecek"
            };

            _context.Payments.Add(payment);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelAppointmentAsync(int appointmentId, int patientId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.DoctorSession)
                .Include(a => a.Payment)
                .FirstOrDefaultAsync(a => a.Id == appointmentId && a.PatientId == patientId);

            if (appointment == null)
            {
                return false;
            }

            if (appointment.DoctorSession != null)
            {
                appointment.DoctorSession.IsAvailable = true;
                _context.DoctorSessions.Update(appointment.DoctorSession);
            }

            if (appointment.Payment != null)
            {
                _context.Payments.Remove(appointment.Payment);
            }

            _context.Appointments.Remove(appointment);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}