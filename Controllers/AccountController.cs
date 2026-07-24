using BeykentHospitalAppointment.Data;
using BeykentHospitalAppointment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeykentHospitalAppointment.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            string fullName,
            string email,
            string password,
            string phoneNumber,
            string? identityNumber)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Bu e-posta adresi ile kayıtlı bir kullanıcı zaten var.";
                return View();
            }

            var user = new User
            {
                FullName = fullName,
                Email = email,
                Password = password,
                Role = "Patient"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var patient = new Patient
            {
                FullName = fullName,
                PhoneNumber = phoneNumber,
                IdentityNumber = identityNumber,
                UserId = user.Id
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetInt32("PatientId", patient.Id);
            HttpContext.Session.SetString("UserFullName", user.FullName);
            HttpContext.Session.SetString("UserRole", user.Role);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users
                .Include(u => u.Patient)
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ViewBag.ErrorMessage = "E-posta veya şifre hatalı.";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserFullName", user.FullName);
            HttpContext.Session.SetString("UserRole", user.Role);

            if (user.Role == "Patient")
            {
                if (user.Patient == null)
                {
                    ViewBag.ErrorMessage = "Bu kullanıcıya bağlı hasta kaydı bulunamadı.";
                    return View();
                }

                HttpContext.Session.SetInt32("PatientId", user.Patient.Id);

                return RedirectToAction("Index", "Home");
            }

            if (user.Role == "Admin")
            {
                HttpContext.Session.Remove("PatientId");

                return RedirectToAction("Index", "Admin");
            }

            ViewBag.ErrorMessage = "Kullanıcı rolü tanımlı değil.";
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}