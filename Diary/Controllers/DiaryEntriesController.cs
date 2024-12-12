using Diary.Data;
using Diary.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diary.Controllers
{
    public class DiaryEntriesController : Controller
    {
        private readonly ApplicationDbContext _db; // nahradit servisní třídou
        public DiaryEntriesController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var objDiaryEntriesList = _db.DiaryEntries.ToList(); // nahradit servisní třídou
            return View(objDiaryEntriesList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
		[HttpPost]
		public IActionResult Create(DiaryEntry obj)
		{

			if (ModelState.IsValid)
			{

				// Uložení záznamu do databáze
				_db.DiaryEntries.Add(obj);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(obj);
		}

		[HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            DiaryEntry diaryEntry = _db.DiaryEntries.Find(id);
            if (diaryEntry == null)
            {
                return NotFound();
            }

            return View(diaryEntry);
        }
        
		public IActionResult Edit(DiaryEntry obj) //metoda pro úpravu již přidaného záznamů v databázi
		{
			if (obj != null && obj.Title.Length < 3) //kontrola na straně serveru
			{
				ModelState.AddModelError("Title", "Title too short");
			}
			if (ModelState.IsValid)
			{
				_db.DiaryEntries.Update(obj);   //Aktualizace záznamu
				_db.SaveChanges();          //Uložení aktualizovaného záznamu
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		[HttpGet]
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			DiaryEntry diaryEntry = _db.DiaryEntries.Find(id);
			if (diaryEntry == null)
			{
				return NotFound();
			}

			return View(diaryEntry);
		}

		public IActionResult Delete(DiaryEntry obj) //metoda pro úpravu již přidaného záznamů v databázi
		{
			
			
				_db.DiaryEntries.Remove(obj);   //Odebrání záznamu
				_db.SaveChanges();          //Uložení odebrání
				return RedirectToAction("Index");
			
			
		}

		

	}
}   
