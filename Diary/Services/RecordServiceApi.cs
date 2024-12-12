using Diary.Data;
using Diary.DTO;
using Diary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

namespace Diary.Services
{
    public class RecordServiceApi
    {
        private ApplicationDbContext _context;

        public RecordServiceApi(ApplicationDbContext context)
        {
            _context = context;
        }

        internal IEnumerable<RecordDtoApi> GetAllRecords()
        {
            var allRecords = _context.RecordEntries;

            var allRecordsDtos = new List<RecordDtoApi>();
            foreach (var item in allRecords)
            {
                allRecordsDtos.Add(ModelToDto(item));
            }
            return allRecordsDtos;
        }

        public RecordDtoApi ModelToDto(RecordEntry item)
        {
            return new RecordDtoApi
            {
                Id = item.Id,
                PhysicalState = item.PhysicalState,
                Created = item.Created,
            };
        }
        public RecordEntry DtoToModel(RecordDtoApi item)
        {

            return new RecordEntry
            {
                Id = item.Id,
                PhysicalState = item.PhysicalState,
                Created = item.Created,
            };
        }

        internal async Task CreateRecordAsync(RecordDtoApi obj)
        {
            await _context.RecordEntries.AddAsync(DtoToModel(obj));
            await _context.SaveChangesAsync();
        }

        internal async Task<RecordDtoApi> GetById(int? id)
        {
            var recordToEdit = await _context.RecordEntries.FirstOrDefaultAsync(student => student.Id == id);
            if (recordToEdit == null)
            {
                return null;
            }
            return ModelToDto(recordToEdit);
        }

        internal async Task UpdateAsync(RecordDtoApi obj)
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
