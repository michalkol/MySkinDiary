﻿using Diary.DTO;
using Diary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Diary.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UsersController : Controller
    {
        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private IPasswordValidator<AppUser> passwordValidator;

        public UsersController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher, IPasswordValidator<AppUser> passwordValidator)
        {
            this.userManager = userManager;
            this.passwordHasher = passwordHasher;
            this.passwordValidator = passwordValidator;
        }
        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]  
        public async Task<IActionResult> CreateAsync(UserDTO newUser)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser()
                {
                    Email = newUser.Email,
                    UserName = newUser.Name,
                };
                IdentityResult identityResult = await userManager.CreateAsync(appUser, newUser.Password);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in identityResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(newUser);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(string id)
        {
            var appUser = await userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                return View("Not found");
            }
            else
            {
                return View(appUser);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(string id, string email, string password)
        {
            AppUser userToEdit = await userManager.FindByIdAsync(id);
            if (userToEdit != null)
            {
                IdentityResult validPassword = null;

                if (!string.IsNullOrEmpty(email))
                {
                    userToEdit.Email = email;
                }
                else
                {
                    ModelState.AddModelError("", "Email cannot be empty");
                }
                if (!string.IsNullOrEmpty(password))
                {
                    validPassword = await passwordValidator.ValidateAsync(userManager, userToEdit, password);
                    userToEdit.PasswordHash = passwordHasher.HashPassword(userToEdit, password);
                }
                else
                {
                    ModelState.AddModelError("", "Password cannot be empty");
                }

                if (validPassword != null && validPassword.Succeeded)
                {
                    IdentityResult result = null;

                    result = await userManager.UpdateAsync(userToEdit);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found");
            }
            return View(userToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            AppUser appUser = await userManager.FindByIdAsync(id);
            if (appUser != null)
            {
                IdentityResult result = await userManager.DeleteAsync(appUser);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found");
            }
            return RedirectToAction("Index");
        }
    }
}