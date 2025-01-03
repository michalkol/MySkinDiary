using Diary.Data;
using Diary.DTO;
using Diary.Models;
using Diary.Services;
using Microsoft.AspNetCore.Mvc;

namespace Diary.Controllers
{
	[ApiController]
	[Route("api/RecordApi")]
	public class RecordApi : ControllerBase
	{
		private readonly RecordService _recordServiceApi;
		private readonly ICurrentUserService _currentUserService;

		public RecordApi(RecordService recordServiceApi, ICurrentUserService currentUserService)
		{
			_recordServiceApi = recordServiceApi;
			_currentUserService = currentUserService;
		}

		[HttpGet]
		public ActionResult Index()
		{
			//// Získání aktuálního uživatelského ID
			//var userId = _currentUserService.UserId;

			//// Filtrování záznamů podle UserId
			var records = _recordServiceApi.GetAllRecords();
			//							   .Where(r => r.UserId == userId);

			return Ok(records);
		}
	}

}

