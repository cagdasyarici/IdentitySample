﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IdentitySample.Controllers
{
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole>_roleManager)
        {
            this.roleManager = _roleManager;
        }

        public IActionResult Index()
        {
            return View(roleManager.Roles);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Required]string name) 
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index","Role");
                }
                else
                {
                    Errors(result);
                }
            }
            return View((object)name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Role");
                }
                else
                {
                    Errors(result);
                }
            }
            else
            {
                ModelState.AddModelError("RoleNotFound_Delete", "Role Not Found");
            }
            return View("Index", roleManager.Roles);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError($"{error.Code} - {error.Description}", error.Description);
            }
        }
    }
}