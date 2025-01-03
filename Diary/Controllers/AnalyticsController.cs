using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Diary.Data;
using Diary.Models;
using Diary.Services;
using Diary.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Diary.Controllers
{
	[Authorize]
	public class AnalyticsController : Controller
	{
		private RecordServiceApi _recordServiceApi;

		public AnalyticsController(RecordServiceApi recordServiceApi)
		{
			_recordServiceApi = recordServiceApi;
		}

		public ActionResult Index()
		{
			var allRecords = _recordServiceApi.GetAllRecords();
			return View(allRecords);
		}

	}
}
