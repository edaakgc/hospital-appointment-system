using BeykentHospitalAppointment.Models;
using BeykentHospitalAppointment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeykentHospitalAppointment.Controllers
{
    public class DoctorSessionsController : Controller
    {
        private readonly IDoctorSessionService _doctorSessionService;
        private readonly IDoctorService _doctorService;

        public DoctorSessionsController(
            IDoctorSessionService doctorSessionService,
            IDoctorService doctorService)
        {
            _doctorSessionService = doctorSessionService;
            _doctorService = doctorService;
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

        private async Task LoadDoctorSelectListAsync(int? selectedDoctorId = null)
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();

            ViewBag.DoctorId = new SelectList(
                doctors,
                "Id",
                "FullName",
                selectedDoctorId);
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            var sessions = await _doctorSessionService.GetAllSessionsAsync();

            return View(sessions);
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

            var session = await _doctorSessionService.GetSessionWithDoctorByIdAsync(id.Value);

            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        public async Task<IActionResult> Create()
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            await LoadDoctorSelectListAsync();

            var session = new DoctorSession
            {
                SessionDate = DateTime.Today,
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(9, 30, 0),
                IsAvailable = true
            };

            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorSession session)
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            if (session.EndTime <= session.StartTime)
            {
                ModelState.AddModelError("EndTime", "Bitiş saati başlangıç saatinden sonra olmalıdır.");
            }

            bool sameSessionExists = await _doctorSessionService.SameSessionExistsAsync(session);

            if (sameSessionExists)
            {
                ModelState.AddModelError("", "Bu doktor için aynı tarih ve saatte zaten seans bulunmaktadır.");
            }

            if (ModelState.IsValid)
            {
                await _doctorSessionService.CreateSessionAsync(session);

                return RedirectToAction(nameof(Index));
            }

            await LoadDoctorSelectListAsync(session.DoctorId);

            return View(session);
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

            var session = await _doctorSessionService.GetSessionByIdAsync(id.Value);

            if (session == null)
            {
                return NotFound();
            }

            await LoadDoctorSelectListAsync(session.DoctorId);

            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DoctorSession session)
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            if (id != session.Id)
            {
                return NotFound();
            }

            if (session.EndTime <= session.StartTime)
            {
                ModelState.AddModelError("EndTime", "Bitiş saati başlangıç saatinden sonra olmalıdır.");
            }

            bool sameSessionExists = await _doctorSessionService.SameSessionExistsAsync(session, session.Id);

            if (sameSessionExists)
            {
                ModelState.AddModelError("", "Bu doktor için aynı tarih ve saatte zaten seans bulunmaktadır.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _doctorSessionService.UpdateSessionAsync(session);
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool exists = await _doctorSessionService.SessionExistsAsync(session.Id);

                    if (!exists)
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await LoadDoctorSelectListAsync(session.DoctorId);

            return View(session);
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

            var session = await _doctorSessionService.GetSessionWithDoctorByIdAsync(id.Value);

            if (session == null)
            {
                return NotFound();
            }

            ViewBag.HasAppointment = await _doctorSessionService.HasAppointmentAsync(session.Id);

            return View(session);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectIfNotAdmin();
            }

            bool hasAppointment = await _doctorSessionService.HasAppointmentAsync(id);

            if (hasAppointment)
            {
                TempData["ErrorMessage"] = "Bu seansa bağlı randevu bulunduğu için seans silinemez.";
                return RedirectToAction(nameof(Index));
            }

            bool deleted = await _doctorSessionService.DeleteSessionAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}