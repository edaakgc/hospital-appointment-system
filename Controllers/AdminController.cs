using BeykentHospitalAppointment.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeykentHospitalAppointment.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminDashboardService _adminDashboardService;

        public AdminController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        private IActionResult RedirectIfNotAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(role))
            {
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("AccessDenied", "Account");
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            ViewBag.DepartmentCount = await _adminDashboardService.GetDepartmentCountAsync();
            ViewBag.DoctorCount = await _adminDashboardService.GetDoctorCountAsync();
            ViewBag.SessionCount = await _adminDashboardService.GetSessionCountAsync();
            ViewBag.AppointmentCount = await _adminDashboardService.GetAppointmentCountAsync();

            return View();
        }
    }
}