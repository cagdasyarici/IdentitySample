using IdentitySample.Models;
using IdentitySample.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace IdentitySample.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;
        public AdminController(UserManager<AppUser> _userManager, IPasswordHasher<AppUser> _passwordHasher)
        {
            this.userManager = _userManager;
            this.passwordHasher = _passwordHasher;
        }

        public IActionResult Index()
        {
            return View(userManager.Users);
        }
        [HttpPost]//Httpdelete gibi yapıları sık kullanıyor muyuz?
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Errors(result);
                }
            }
            else
                ModelState.AddModelError("UserNotFound_Delete", "User not found");
            return View("Index", userManager.Users);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Update(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
            {
                return RedirectToAction("Index","Admin");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string password)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                else
                    ModelState.AddModelError("UpdateUser", "Email cannot be empty");
                if (!string.IsNullOrEmpty(password))
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                else
                    ModelState.AddModelError("UpdateUser", "Password cannot be empty");
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Errors(result);
                    }

                }

            }
            else
                ModelState.AddModelError("UserNotFound", "User Not Found");
            return View(user);
        }
        private void Errors(IdentityResult result) 
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError($"{error.Code} - {error.Description}", error.Description);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user) 
        {
            if(ModelState.IsValid)
            {
                AppUser appUser = new() {
                    UserName = user.Name,
                    Email = user.Email,
                };
                IdentityResult result = await userManager.CreateAsync(appUser,user.Password);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("UserCreateErr", error.Description);
                    }
                }
            }
            return View(user);
        }
    }
}
