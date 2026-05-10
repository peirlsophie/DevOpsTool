using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using DevOpsEduTool.Data;
using DevOpsEduTool.Models;

namespace DevOpsEduTool.Controllers
{
    public class AccountController : Controller
    {
        private readonly DevOpsEduContext _context;

        public AccountController(DevOpsEduContext context)
        {
            _context = context;
        }

        // Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("UserId", user.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Wrong username or password";
            return RedirectToAction("Index", "Home");
        }

        // Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        //Register 
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User newUser, string password)
        {

            if (_context.Users.Any(u => u.Username == newUser.Username || u.Email == newUser.Email))
            {
                ModelState.AddModelError("", "Username or Email is already taken.");
                return View(newUser);
            }

            newUser.PasswordHash = password;

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}