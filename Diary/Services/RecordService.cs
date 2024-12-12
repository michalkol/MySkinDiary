using Diary.Data;
using Diary.DTO;
using Diary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

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

        public RecordDTO ModelToDto(RecordEntry item)
        {
            return new RecordDTO
            {
                Id = item.Id,
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

        internal async Task CreateRecordAsync(RecordDTO obj)
        {
            await _context.RecordEntries.AddAsync(DtoToModel(obj));
            await _context.SaveChangesAsync();
        }

        internal async Task<RecordDTO> GetById(int? id)
        {
            var recordToEdit = await _context.RecordEntries.FirstOrDefaultAsync(student => student.Id == id);
            if (recordToEdit == null)
            {
                return null;
            }
            return ModelToDto(recordToEdit);
        }

        internal async Task UpdateAsync(RecordDTO obj)
        {
            _context.Update(DtoToModel(obj));
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
