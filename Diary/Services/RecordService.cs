using Diary.Data;
using Diary.DTO;
using Diary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using System.Runtime.CompilerServices;

namespace Diary.Services
{
    public class RecordService
    {
        private ApplicationDbContext _context;

        public RecordService(ApplicationDbContext context)
        {
            _context = context;
        }

        internal IEnumerable<RecordDTO> GetAllRecords()
        {
            var allRecords = _context.RecordEntries;

            var allRecordsDtos = new List<RecordDTO>();
            foreach (var item in allRecords)
            {
                allRecordsDtos.Add(ModelToDto(item));
            }
            return allRecordsDtos;
        }

		internal IQueryable<RecordDTO> searchService()
		{
            return _context.RecordEntries.Select(item => new RecordDTO
            {
                // Můžete zde nastavit vlastnosti DTO
                Id = item.Id,
                UserId = item.UserId,
                PhysicalState = item.PhysicalState,
                PhysicalDesc = item.PhysicalDesc,
                MentalState = item.MentalState,
                MentalDesc = item.MentalDesc,
                SkinState1 = item.SkinState1,
                SkinState2 = item.SkinState2,
                SkinState3 = item.SkinState3,
                SkinState4 = item.SkinState4,
                SkinState5 = item.SkinState5,
                SkinStateDesc = item.SkinStateDesc,
                IsSportActivity = item.IsSportActivity,
                SportActivityDesc = item.SportActivityDesc,
                IsSexActivity = item.IsSexActivity,
                IsAlcohol = item.IsAlcohol,
                Digesting = item.Digesting,
                DietDesc = item.DietDesc,
                Menstruation = item.Menstruation,
                MedicationDesc = item.MedicationDesc,
                Created = item.Created,
                Photo = item.Photo,
            });
		}

		public RecordDTO ModelToDto(RecordEntry item)
        {
            return new RecordDTO
            {
                Id = item.Id,
                UserId = item.UserId,
                PhysicalState = item.PhysicalState,
                PhysicalDesc = item.PhysicalDesc,
                MentalState = item.MentalState,
                MentalDesc = item.MentalDesc,
                SkinState1 = item.SkinState1,
                SkinState2 = item.SkinState2,
                SkinState3 = item.SkinState3,
                SkinState4 = item.SkinState4,
                SkinState5 = item.SkinState5,
                SkinStateDesc = item.SkinStateDesc,
                IsSportActivity = item.IsSportActivity,
                SportActivityDesc = item.SportActivityDesc,
                IsSexActivity = item.IsSexActivity,
                IsAlcohol = item.IsAlcohol,
                Digesting = item.Digesting,
                DietDesc = item.DietDesc,
                Menstruation = item.Menstruation,
                MedicationDesc = item.MedicationDesc,
                Created = item.Created,
                Photo = item.Photo,
            };
        }
        public RecordEntry DtoToModel(RecordDTO item)
        {

            return new RecordEntry
            {
                Id = item.Id,
                UserId = item.UserId,
                PhysicalState = item.PhysicalState,
                PhysicalDesc = item.PhysicalDesc,
                MentalState = item.MentalState,
                MentalDesc = item.MentalDesc,
                SkinState1 = item.SkinState1,
                SkinState2 = item.SkinState2,
                SkinState3 = item.SkinState3,
                SkinState4 = item.SkinState4,
                SkinState5 = item.SkinState5,
                SkinStateDesc = item.SkinStateDesc,
                IsSportActivity = item.IsSportActivity,
                SportActivityDesc = item.SportActivityDesc,
                IsSexActivity = item.IsSexActivity,
                IsAlcohol = item.IsAlcohol,
                Digesting = item.Digesting,
                DietDesc = item.DietDesc,
                Menstruation = item.Menstruation,
                MedicationDesc = item.MedicationDesc,
                Created = item.Created,
                Photo = item.Photo,
            };
        }

        internal async Task CreateRecordAsync(RecordDTO obj /*IFormFile pic*/)
        {
			//if (pic != null && pic.Length > 0)
			//{
			//	obj.Photo = await PhotoToByteArrayAsync(pic);
			//}
			await _context.RecordEntries.AddAsync(DtoToModel(obj));
            await _context.SaveChangesAsync();
        }

		//internal async Task<byte[]> PhotoToByteArrayAsync(IFormFile pic)
		//{
		//	using (var memoryStream = new MemoryStream())
		//	{
		//		await pic.CopyToAsync(memoryStream);
		//		return memoryStream.ToArray();
		//	}
		//}

		internal async Task<RecordDTO> GetById(int? id)
        {
            var recordToEdit = await _context.RecordEntries.FirstOrDefaultAsync(rec => rec.Id == id);
            if (recordToEdit == null)
            {
                return null;
            }
            return ModelToDto(recordToEdit);
        }

        internal async Task UpdateAsync(RecordDTO obj/*, IFormFile Photo*/)
        {
            //if (Photo != null && Photo.Length > 0)
            //{
            //    obj.Photo = await PhotoToByteArrayAsync(Photo);
            //}

            // přidán přístup přes model (zkontrolovat) !!!!!

			_context.RecordEntries.Update(DtoToModel(obj));
            await _context.SaveChangesAsync();
        }

        internal async Task DeleteAsync(int id)
        {
            var recordToDelete = await _context.RecordEntries.FirstOrDefaultAsync(x => x.Id == id);
            _context.Remove(recordToDelete);
            await _context.SaveChangesAsync();
        }
    }


     
}
