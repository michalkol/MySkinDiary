using Diary.Data;
using Diary.DTO;
using Diary.Models;
using Diary.Services;
using Microsoft.AspNetCore.Mvc;

namespace Diary.Controllers
{
	[ApiController]
    [Route("api/[Controller]")]
    public class RecordApi : ControllerBase
	{
		private RecordService _recordServiceApi;
		

		public RecordApi(RecordService recordServiceApi)
		{
			_recordServiceApi = recordServiceApi;
		}

        [HttpGet] 
        public ActionResult Index()
        {
            var records = _recordServiceApi.GetAllRecords();
            return Ok(records);  
        }

    }
}

