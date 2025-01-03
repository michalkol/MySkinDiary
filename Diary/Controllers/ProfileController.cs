using Diary.DTO;
using Diary.Models;
using Diary.Services;
using Diary.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diary.Controllers
{
	public class ProfileController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IPasswordHasher<AppUser> _passwordHasher;
		private readonly IPasswordValidator<AppUser> _passwordValidator;
		private readonly ICurrentUserService _currentUserService;
		private readonly AvatarService _avatarService;

		public ProfileController(
			UserManager<AppUser> userManager,
			IPasswordHasher<AppUser> passwordHasher,
			IPasswordValidator<AppUser> passwordValidator,
			ICurrentUserService currentUserService,
			AvatarService avatarService)
		{
			_userManager = userManager;
			_passwordHasher = passwordHasher;
			_passwordValidator = passwordValidator;
			_currentUserService = currentUserService;
			_avatarService = avatarService;
		}

		public async Task<IActionResult> Index()
		{
			var userId = _currentUserService.UserId; // Získání aktuálního UserId
			var user = await _userManager.FindByIdAsync(userId); // Načtení aktuálního uživatele

			if (user == null)
			{
				return NotFound();
			}

			var avatar = await _avatarService.GetById(userId);
			var userProfile = new UserProfileViewModel
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.UserName,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Photo = avatar?.Photo
			};

			return View(userProfile);
		}

		[HttpGet]
		public async Task<IActionResult> Edit()
		{
			var userId = _currentUserService.UserId;
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
				return NotFound();
			}

			var avatar = await _avatarService.GetById(userId);
			var userProfile = new UserProfileViewModel
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.UserName,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Photo = avatar?.Photo
			};

			return View(userProfile);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(UserProfileViewModel model, IFormFile? photoFile)
		{
			var userId = _currentUserService.UserId;
			var userToEdit = await _userManager.FindByIdAsync(userId);

			if (userToEdit == null)
			{
				return NotFound();
			}

			if (photoFile != null && photoFile.Length > 0)
			{
				using (var memoryStream = new MemoryStream())
				{
					await photoFile.CopyToAsync(memoryStream);
					var avatarDto = new AvatarDTO
					{
						UserId = userId,
						Photo = memoryStream.ToArray()
					};

					var existingAvatar = await _avatarService.GetById(userId);
					if (existingAvatar != null)
					{
						existingAvatar.Photo = avatarDto.Photo;
						await _avatarService.UpdateAsync(existingAvatar);
					}
					else
					{
						await _avatarService.CreateRecordAsync(avatarDto);
					}
				}
			}

			userToEdit.Email = model.Email;
			userToEdit.UserName = model.UserName;
			userToEdit.FirstName = model.FirstName;
			userToEdit.LastName = model.LastName;

			if (!string.IsNullOrEmpty(model.Password))
			{
				var validPassword = await _passwordValidator.ValidateAsync(_userManager, userToEdit, model.Password);
				if (!validPassword.Succeeded)
				{
					foreach (var error in validPassword.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
					return View(model);
				}

				userToEdit.PasswordHash = _passwordHasher.HashPassword(userToEdit, model.Password);
			}

			var result = await _userManager.UpdateAsync(userToEdit);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return View(model);
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> Delete()
		{
			var userId = _currentUserService.UserId;
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
				return NotFound();
			}

			var result = await _userManager.DeleteAsync(user);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return RedirectToAction("Index");
			}

			return RedirectToAction("Index", "Home");
		}
	}
}
