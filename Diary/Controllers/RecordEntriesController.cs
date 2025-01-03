using Diary.Data;
using Diary.DTO;
using Diary.Models;
using Diary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Diary.Controllers
{
	[Authorize]
	public class RecordEntriesController : Controller
	{
		private readonly ICurrentUserService _currentUserService;
		private RecordService _recordService;
	

		public RecordEntriesController(RecordService recordService, ICurrentUserService currentUserService)
		{
            _recordService = recordService;
			_currentUserService = currentUserService;
		}

		[HttpGet]
		public ActionResult Index()
		{
			var allRecords = _recordService.GetAllRecords().Where(e => e.UserId == _currentUserService.UserId);
			return View(allRecords);
		}


		[HttpGet]
        public IActionResult Create()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync(RecordDTO obj, IFormFile? photoFile)
		{
			if (photoFile != null)
			{
				using (var memoryStream = new MemoryStream())
				{
					await photoFile.CopyToAsync(memoryStream);
					obj.Photo = memoryStream.ToArray();
				}
			}
			
			obj.UserId = _currentUserService.UserId;
			await _recordService.CreateRecordAsync(obj);
			return RedirectToAction("Index");
		}

		public int GetCount(int _recordCount)
		{
			var totalCount = _recordCount;
			return totalCount;
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

		public async Task<IActionResult> EditAsync(RecordDTO obj, IFormFile? photoFile2) 
		{
			

			if (photoFile2 != null)
			{
				using (var memoryStream = new MemoryStream())
				{
					await photoFile2.CopyToAsync(memoryStream);
					obj.Photo = memoryStream.ToArray();
				}
			}
			
			obj.UserId = _currentUserService.UserId;
			await _recordService.UpdateAsync(obj);

			return RedirectToAction("Index");
		}

		

		[HttpGet]
		public async Task<IActionResult> DetailAsync(int? id)
		{
			var recordDetail = await _recordService.GetById(id);
			if (recordDetail == null)
			{
				return NotFound();
			}
			return View(recordDetail);
		}

		public async Task<IActionResult> DeleteAsync(int id) 
		{
			var recordToDelete =await _recordService.GetById(id);
            if (recordToDelete == null)
            {
                return View("NotFound");
            }
			await _recordService.DeleteAsync(id);
            return RedirectToAction("Index");


		}

		public async Task<IActionResult> DisplayPhotoAsync(int id)
		{
			var record = await _recordService.GetById(id);
			if (record != null && record.Photo != null)
			{
				return File(record.Photo, "image/jpeg");  // Vrací obrázek v formátu JPEG
			}
			return NotFound();
		}
		[HttpGet]
		public async Task<IActionResult> Search(string? searchQuery)
		{
			// Uložení vyhledávacího dotazu do ViewData (pro zachování hodnoty ve formuláři)
			ViewData["SearchQuery"] = searchQuery;

			// Načtení všech záznamů z databáze
			var records = _recordService.searchService();

			// Pokud je zadán vyhledávací dotaz, filtrujeme záznamy
			if (!string.IsNullOrEmpty(searchQuery))
			{
				records = records.Where(r =>

					r.PhysicalDesc.Contains(searchQuery) ||
					r.MentalDesc.Contains(searchQuery) ||
					r.DietDesc.Contains(searchQuery) ||
					r.MedicationDesc.Contains(searchQuery) ||
					r.SkinStateDesc.Contains(searchQuery));
			}

			// Vrácení filtrovaných výsledků
			return View("Index", await records.ToListAsync());
		}

	}
}
