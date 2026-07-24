using BeykentHospitalAppointment.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeykentHospitalAppointment.Controllers
{
    public class PatientAppointmentsController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IDoctorService _doctorService;
        private readonly IDoctorSessionService _doctorSessionService;
        private readonly IAppointmentService _appointmentService;

        public PatientAppointmentsController(
            IDepartmentService departmentService,
            IDoctorService doctorService,
            IDoctorSessionService doctorSessionService,
            IAppointmentService appointmentService)
        {
            _departmentService = departmentService;
            _doctorService = doctorService;
            _doctorSessionService = doctorSessionService;
            _appointmentService = appointmentService;
        }

        private string? GetCurrentUserRole()
        {
            return HttpContext.Session.GetString("UserRole");
        }

        private int? GetCurrentPatientId()
        {
            return HttpContext.Session.GetInt32("PatientId");
        }

        private bool IsPatient()
        {
            return GetCurrentUserRole() == "Patient";
        }

        private bool IsAdmin()
        {
            return GetCurrentUserRole() == "Admin";
        }

        private IActionResult RedirectIfNotLoggedIn()
        {
            return RedirectToAction("Login", "Account");
        }

        private IActionResult RedirectIfUnauthorized()
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        public async Task<IActionResult> Index()
        {
            var role = GetCurrentUserRole();

            if (string.IsNullOrEmpty(role))
            {
                return RedirectIfNotLoggedIn();
            }

            if (IsAdmin())
            {
                ViewBag.PageTitle = "Tüm Randevular";
                ViewBag.IsAdminView = true;

                var allAppointments = await _appointmentService.GetAllAppointmentsAsync();

                return View(allAppointments);
            }

            if (IsPatient())
            {
                var patientId = GetCurrentPatientId();

                if (patientId == null)
                {
                    return RedirectIfNotLoggedIn();
                }

                ViewBag.PageTitle = "Randevularım";
                ViewBag.IsAdminView = false;

                var myAppointments = await _appointmentService.GetAppointmentsByPatientAsync(patientId.Value);

                return View(myAppointments);
            }

            return RedirectIfUnauthorized();
        }

        public async Task<IActionResult> ChooseDepartment()
        {
            if (IsAdmin())
            {
                return RedirectIfUnauthorized();
            }

            if (!IsPatient())
            {
                return RedirectIfNotLoggedIn();
            }

            var departments = await _departmentService.GetAllDepartmentsAsync();

            return View(departments);
        }

        public async Task<IActionResult> SelectDoctor(int departmentId)
        {
            if (IsAdmin())
            {
                return RedirectIfUnauthorized();
            }

            if (!IsPatient())
            {
                return RedirectIfNotLoggedIn();
            }

            var department = await _departmentService.GetDepartmentByIdAsync(departmentId);

            if (department == null)
            {
                return NotFound();
            }

            ViewBag.DepartmentName = department.Name;

            var doctors = await _doctorService.GetDoctorsByDepartmentAsync(departmentId);

            return View(doctors);
        }

        public async Task<IActionResult> SelectSession(int doctorId)
        {
            if (IsAdmin())
            {
                return RedirectIfUnauthorized();
            }

            if (!IsPatient())
            {
                return RedirectIfNotLoggedIn();
            }

            var doctor = await _doctorService.GetDoctorWithDepartmentByIdAsync(doctorId);

            if (doctor == null)
            {
                return NotFound();
            }

            ViewBag.DoctorName = doctor.FullName;
            ViewBag.DepartmentName = doctor.Department?.Name;

            var sessions = await _doctorSessionService.GetAvailableSessionsByDoctorAsync(doctorId);

            return View(sessions);
        }

        public async Task<IActionResult> Create(int sessionId)
        {
            if (IsAdmin())
            {
                return RedirectIfUnauthorized();
            }

            if (!IsPatient())
            {
                return RedirectIfNotLoggedIn();
            }

            var session = await _doctorSessionService.GetAvailableSessionForBookingAsync(sessionId);

            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int sessionId, string paymentType)
        {
            if (IsAdmin())
            {
                return RedirectIfUnauthorized();
            }

            if (!IsPatient())
            {
                return RedirectIfNotLoggedIn();
            }

            var patientId = GetCurrentPatientId();

            if (patientId == null)
            {
                return RedirectIfNotLoggedIn();
            }

            bool created = await _appointmentService.CreateAppointmentAsync(
                patientId.Value,
                sessionId,
                paymentType);

            if (!created)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            if (IsAdmin())
            {
                return RedirectIfUnauthorized();
            }

            if (!IsPatient())
            {
                return RedirectIfNotLoggedIn();
            }

            var patientId = GetCurrentPatientId();

            if (patientId == null)
            {
                return RedirectIfNotLoggedIn();
            }

            bool cancelled = await _appointmentService.CancelAppointmentAsync(id, patientId.Value);

            if (!cancelled)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}