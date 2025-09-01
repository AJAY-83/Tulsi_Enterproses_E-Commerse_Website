using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sample_LadiesClothings.Models;
using System.Security.Claims;

namespace Sample_LadiesClothings.Controllers
{
    public class AccountController : Controller
    {

        private readonly MyContext _db;
        private readonly PasswordHasher<Customer> _hasher = new();

        public AccountController(MyContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            bool exists = await _db.tbl_customer.AnyAsync(c => c.Customer_Email == vm.Email);
            if (exists)
            {
                ModelState.AddModelError(nameof(vm.Email), "Email already registered.");
                return View(vm);
            }

            var customer = new Customer
            {
                Customer_Name = vm.Name,
                Customer_Email = vm.Email
            };
            customer.Customer_PasswordHash = _hasher.HashPassword(customer, vm.Password);
            _db.tbl_customer.Add(customer);
            await _db.SaveChangesAsync();

            await SignInAsync(customer);
            return RedirectToAction("Index", "Products");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var user = await _db.tbl_customer.FirstOrDefaultAsync(c => c.Customer_Email == vm.Email);
            if (user == null)
            {
                ViewBag.message = "Invalid credentials";
                ModelState.AddModelError(string.Empty, "Invalid credentials");
                return View(vm);
            }
            if (string.IsNullOrEmpty(user.Customer_PasswordHash))
            {
                ViewBag.message = "Invalid credentials";
                ModelState.AddModelError(string.Empty, "Invalid credentials");
                return View(vm);
            }
            var result = _hasher.VerifyHashedPassword(user, user.Customer_PasswordHash, vm.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials");
                ViewBag.message = "Invalid credentials";
                return View(vm);
            }
            await SignInAsync(user);
            return RedirectToAction("Index", "Products");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private async Task SignInAsync(Customer c)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, c.Customer_Id.ToString()),
                new Claim(ClaimTypes.Name, c.Customer_Name),
                new Claim(ClaimTypes.Email, c.Customer_Email)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            if (c.Customer_Id>0)
                HttpContext.Session.SetInt32("Customer_Id", c.Customer_Id);
            HttpContext.Session.SetString("Customer_Name", c.Customer_Name);
        }
    }
}
