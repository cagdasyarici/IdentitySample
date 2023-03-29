using IdentitySample.Models;
using IdentitySample.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentitySample.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;

        public AccountController(SignInManager<AppUser> _signInManager, UserManager<AppUser> _userManager)
        {
            this.signInManager = _signInManager;
            this.userManager = _userManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login();
            login.ReturnUrl = returnUrl;
            return View(login);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if(ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(login.Email);

                if (user != null) 
                {
                    await signInManager.SignOutAsync();
                    //Microsoft.AspNetCore.Identity.SignInResult signInResult = await signInManager.PasswordSignInAsync(user,login.Password,false,false);
                    Microsoft.AspNetCore.Identity.SignInResult signInResult = await signInManager.PasswordSignInAsync(user, login.Password, login.Remember, false);

                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("LoginFailur","Login Failed. Invalid Email or Password");
                }
            }
            return View(login);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
