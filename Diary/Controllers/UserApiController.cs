using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Diary.Controllers
{
	public class UserApiController : Controller
	{
		[HttpGet("api/current-user")]
		public IActionResult GetCurrentUser()
		{
			if (User.Identity.IsAuthenticated)
			{
				var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				var userName = User.Identity.Name;
				return Ok(new { UserId = userId, UserName = userName });
			}
			return Unauthorized();
		}

	}
}
