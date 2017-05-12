using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SignalmanPortal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalmanPortal.Controllers
{
    [Authorize]
    public class SecretController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SecretController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Secret model)
        {
            if (model.secretCode == "1qAZXSw2")
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (!HttpContext.User.IsInRole("Administrator"))
                {
                    await _userManager.AddToRoleAsync(user, "Administrator");
                    await _signInManager.RefreshSignInAsync(user);
                }
            }

            return RedirectToAction("Index", "Admin", null);
        }
    }
}
