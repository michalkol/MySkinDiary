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
    public class EventEntriesController : Controller
    {

        private readonly ICurrentUserService _currentUserService;
        private EventService _eventService;

        public EventEntriesController(EventService eventService, ICurrentUserService currentUserService)
        {
            _eventService = eventService;
            _currentUserService = currentUserService;
        }

        public IActionResult GetEvents()
        {
            // Získání všech událostí pomocí servisní metody
            var eventDtos = _eventService.GetAllRecords().Where(e => e.UserId == _currentUserService.UserId);

            // Převod na formát kompatibilní s FullCalendar
            var events = eventDtos.Select(e => new
            {
                id = e.Id,
                title = e.Description,
                start = e.Start.ToString("yyyy-MM-ddTHH:mm:ss"),
                end = e.End.ToString("yyyy-MM-ddTHH:mm:ss")
            });

            return Json(events);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var allRecords = _eventService.GetAllRecords().Where(e => e.UserId == _currentUserService.UserId);
            return View(allRecords);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(EventDTO obj, IFormFile? photoFile)
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
            await _eventService.CreateRecordAsync(obj);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int? id)
        {
            var recordToEdit = await _eventService.GetById(id);
            if (recordToEdit == null)
            {
                return NotFound();
            }
            return View(recordToEdit);
        }

        public async Task<IActionResult> EditAsync(EventDTO obj, IFormFile? photoFile2)
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
            await _eventService.UpdateAsync(obj);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DetailAsync(int? id)
        {
            var recordDetail = await _eventService.GetById(id);
            if (recordDetail == null)
            {
                return NotFound();
            }
            return View(recordDetail);
        }

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var recordToDelete = await _eventService.GetById(id);
            if (recordToDelete == null)
            {
                return View("NotFound");
            }
            await _eventService.DeleteAsync(id);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> DisplayPhotoAsync(int id)
        {
            var record = await _eventService.GetById(id);
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
            var records = _eventService.searchService();

            // Pokud je zadán vyhledávací dotaz, filtrujeme záznamy
            if (!string.IsNullOrEmpty(searchQuery))
            {
                records = records.Where(r =>

                    r.Description.Contains(searchQuery) ||
                    r.Location.Contains(searchQuery));
            }

            // Vrácení filtrovaných výsledků
            return View("Index", await records.ToListAsync());
        }
    }
}
