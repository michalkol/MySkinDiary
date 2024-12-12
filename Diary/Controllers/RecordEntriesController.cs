using Diary.Data;
using Diary.DTO;
using Diary.Models;
using Diary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Diary.Controllers
{
	[Authorize]
	public class RecordEntriesController : Controller
	{
		private RecordService _recordService;

		public RecordEntriesController(RecordService recordService)
		{
            _recordService = recordService;
		}
        [HttpGet]
        public ActionResult Index()
		{
			var allRecords = _recordService.GetAllRecords();
			return View(allRecords);
		}
		[HttpGet]
        public IActionResult Create()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync(RecordDTO obj)
		{
				// Uložení záznamu do databáze
				await _recordService.CreateRecordAsync(obj);
				return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> EditAsync(int? id)
		{
			var recordToEdit = await _recordService.GetById(id);
			if (recordToEdit == null)
			{
				return NotFound();
			}
			return View(recordToEdit);
		}

		public async Task<IActionResult> EditAsync(RecordDTO obj) //metoda pro úpravu již přidaného záznamů v databázi
		{
				await _recordService.UpdateAsync(obj);
				return RedirectToAction("Index");
		}

		

		public async Task<IActionResult> DeleteAsync(int id) //metoda pro úpravu již přidaného záznamů v databázi
		{
			var recordToDelete =await _recordService.GetById(id);   //Odebrání záznamu       //Uložení odebrání
            if (recordToDelete == null)
            {
                return View("NotFound");
            }
            await _recordService.DeleteAsync(id);
            return RedirectToAction("Index");


		}
	}
}
