using AuthApp.Data;
using AuthApp.Models;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace AuthApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Account/SignUp
        public IActionResult SignUp() => View();

        // POST: /Account/SignUp
        [HttpPost]
        public IActionResult SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (_db.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already registered.");
                return View(model);
            }

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                CreatedAt = DateTime.Now
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            TempData["Success"] = "Account created! Please sign in.";
            return RedirectToAction("SignIn");
        }

        // GET: /Account/SignIn
        public IActionResult SignIn() => View();

        // POST: /Account/SignIn
        [HttpPost]
        public IActionResult SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _db.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserEmail", user.Email);

            return RedirectToAction("Index", "Dashboard");
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn");
        }
    }
}