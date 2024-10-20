using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EJ.SistemaAgendamentos.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signinManager;

        public AuthController(ILogger<AuthController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signinManager = signinManager;

            ViewData["hideHeader"] = "0";
        }

        [HttpGet(Name = "Login")]
        public IActionResult Login()
        {
            ViewData["hideHeader"] = true;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm]string email, [FromForm]string password)
        {
            ViewData["hideHeader"] = true;
            var result = await _signinManager.PasswordSignInAsync(email, password, false, false);

            if(result.Succeeded){
                return RedirectToAction("Index", "Home");
            }
            else{
                TempData["Errors"] = "Usuário e/ou senha incorretos.";
            }

            return View();
        }

        public IActionResult Register()
        {
            ViewData["hideHeader"] = true;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm]string email, [FromForm]string password, [FromForm]string confirmPassword)
        {
            ViewData["hideHeader"] = true;

            var identityUser = new IdentityUser{
                UserName = email,
                Email = email,           
            };


            var result = await _userManager.CreateAsync(identityUser, password);

            if(result.Succeeded){
                await _signinManager.PasswordSignInAsync(email, password, true, false);
                return RedirectToAction("Index", "Home");
            }
            else{
                TempData["Errors"] = String.Join(",", result.Errors.Select(x => x.Description));
            }
            return View();
        }

        public async Task<IActionResult> Logout() {
            await _signinManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}