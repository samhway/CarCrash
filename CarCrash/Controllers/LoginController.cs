using CarCrash.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Controllers
{
    public class LoginController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;

        public LoginController(UserManager<IdentityUser> um, SignInManager<IdentityUser> sim)
        {
            userManager = um;
            signInManager = sim;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new User { });
        }

        [HttpPost]
        public async Task<IActionResult> Login(User u)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(u.UserName);

                if (user != null)
                {
                    await signInManager.SignOutAsync();

                    if ((await signInManager.PasswordSignInAsync(user, u.Password, false, false)).Succeeded)
                    {
                        if (u.AdminAccess == true)
                        {
                            return Redirect("/admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }

                }

            }
            ModelState.AddModelError("", "Invalid username or password");
            return View(u);
        }

        public async Task<RedirectResult> Logout (string returnUrl = "/")
        {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
