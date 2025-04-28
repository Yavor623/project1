﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AccountManagement.Models;
using AccountManagement.Models.Accounts;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;

namespace AccountManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IActionResult Index()
        {
            return View();
        }
        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _roleManager= roleManager;
            _userManager= userManager;
            _signInManager= signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid) 
            {
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(model.Email,model.Password,model.RememberMe,false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }

            ModelState.AddModelError(string.Empty, "Грешен имейл или парола.");

            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser 
                { 
                    UserName = model.Email, 
                    Email = model.Email ,
                    FirstName = model.FirstName ,
                    LastName = model.LastName ,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user,model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user,isPersistent:false);

                    return RedirectToAction("Index", "Home");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
