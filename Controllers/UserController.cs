using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using JustManAdmin.Helpers;
using JustManAdmin.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JustManAdmin.Controllers
{
    public class UserController : Controller
    {
        private DataContext context;
        public UserController(DataContext _context)
        {
            this.context = _context;
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(User model)
        {
            model.Password = Hashing.CreateMD5(model.Password);
            var user = await context.Users
                .Include(p=>p.Role)
                .FirstOrDefaultAsync(p =>
                    p.Login == model.Login &&
                    p.Password == model.Password);
            if (user != null)
            {
                await Authenticate(user);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(User model)
        {
            model.Password = Hashing.CreateMD5(model.Password);
            model.Role = await context.Roles.FirstAsync(p=> p.Id == 2);
            var user = await context.Users
                .FirstOrDefaultAsync(p =>
                    p.Login == model.Login);
            if (user == null)
            {
                context.Users.Add(model);
                await context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public async Task Authenticate(User model)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, model.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, model.Role.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SignIn");
        }
    }
}