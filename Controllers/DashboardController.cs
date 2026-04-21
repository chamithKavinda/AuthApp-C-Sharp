using Microsoft.AspNetCore.Mvc;

namespace AuthApp.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // Guard: redirect to login if not authenticated
            if (HttpContext.Session.GetString("UserName") == null)
                return RedirectToAction("SignIn", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");
            return View();
        }
    }
}