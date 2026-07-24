using BeykentHospitalAppointment.Models;
using BeykentHospitalAppointment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeykentHospitalAppointment.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IDepartmentService _departmentService;

        public DoctorsController(
            IDoctorService doctorService,
            IDepartmentService departmentService)
        {
            _doctorService = doctorService;
            _departmentService = departmentService;
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

        private async Task LoadDepartmentSelectListAsync(int? selectedDepartmentId = null)
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();

            ViewBag.DepartmentId = new SelectList(
                departments,
                "Id",
                "Name",
                selectedDepartmentId);
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            var doctors = await _doctorService.GetAllDoctorsAsync();

            return View(doctors);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _doctorService.GetDoctorWithDepartmentByIdAsync(id.Value);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        public async Task<IActionResult> Create()
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            await LoadDepartmentSelectListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            if (ModelState.IsValid)
            {
                await _doctorService.CreateDoctorAsync(doctor);

                return RedirectToAction(nameof(Index));
            }

            await LoadDepartmentSelectListAsync(doctor.DepartmentId);

            return View(doctor);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _doctorService.GetDoctorByIdAsync(id.Value);

            if (doctor == null)
            {
                return NotFound();
            }

            await LoadDepartmentSelectListAsync(doctor.DepartmentId);

            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doctor doctor)
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _doctorService.UpdateDoctorAsync(doctor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool exists = await _doctorService.DoctorExistsAsync(doctor.Id);

                    if (!exists)
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await LoadDepartmentSelectListAsync(doctor.DepartmentId);

            return View(doctor);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _doctorService.GetDoctorWithDepartmentByIdAsync(id.Value);

            if (doctor == null)
            {
                return NotFound();
            }

            ViewBag.HasSessions = await _doctorService.HasSessionsAsync(doctor.Id);
            ViewBag.HasAppointments = await _doctorService.HasAppointmentsAsync(doctor.Id);

            return View(doctor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            bool hasSessions = await _doctorService.HasSessionsAsync(id);
            bool hasAppointments = await _doctorService.HasAppointmentsAsync(id);

            if (hasSessions || hasAppointments)
            {
                TempData["ErrorMessage"] = "Bu doktora bağlı seans veya randevu bulunduğu için doktor silinemez.";
                return RedirectToAction(nameof(Index));
            }

            bool deleted = await _doctorService.DeleteDoctorAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}