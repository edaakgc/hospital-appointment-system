using BeykentHospitalAppointment.Models;
using BeykentHospitalAppointment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeykentHospitalAppointment.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
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

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            var departments = await _departmentService.GetAllDepartmentsAsync();

            return View(departments);
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

            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            if (ModelState.IsValid)
            {
                await _departmentService.CreateDepartmentAsync(department);

                return RedirectToAction(nameof(Index));
            }

            return View(department);
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

            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _departmentService.UpdateDepartmentAsync(department);
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool exists = await _departmentService.DepartmentExistsAsync(department.Id);

                    if (!exists)
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(department);
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

            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);

            if (department == null)
            {
                return NotFound();
            }

            ViewBag.HasDoctors = await _departmentService.HasDoctorsAsync(department.Id);

            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            bool hasDoctors = await _departmentService.HasDoctorsAsync(id);

            if (hasDoctors)
            {
                TempData["ErrorMessage"] = "Bu branşa bağlı doktor bulunduğu için branş silinemez.";
                return RedirectToAction(nameof(Index));
            }

            bool deleted = await _departmentService.DeleteDepartmentAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}