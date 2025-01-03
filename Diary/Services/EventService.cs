using Diary.Data;
using Diary.DTO;
using Diary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using System.Runtime.CompilerServices;

namespace Diary.Services
{
    public class EventService
    {
        private ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        internal IEnumerable<EventDTO> GetAllRecords()
        {
            var allRecords = _context.EventEntries;

            var allRecordsDtos = new List<EventDTO>();
            foreach (var item in allRecords)
            {
                allRecordsDtos.Add(ModelToDto(item));
            }
            return allRecordsDtos;
        }

        internal IQueryable<EventDTO> searchService()
        {
            return _context.EventEntries.Select(item => new EventDTO
            {
                Id = item.Id,
                UserId = item.UserId,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                Description = item.Description,
                Location = item.Location,
                Start = item.Start,
                End = item.End,
                Photo = item.Photo,
            });
        }

        public EventDTO ModelToDto(EventEntry item)
        {
            return new EventDTO
            {
                Id = item.Id,
                UserId = item.UserId,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                Description = item.Description,
                Location = item.Location,
                Start = item.Start,
                End = item.End,
                Photo = item.Photo,
            };
        }
        public EventEntry DtoToModel(EventDTO item)
        {
            return new EventEntry
            {
                Id = item.Id,
                UserId = item.UserId,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                Description = item.Description,
                Location = item.Location,
                Start = item.Start,
                End = item.End,
                Photo = item.Photo,
            };
        }
        internal async Task CreateRecordAsync(EventDTO obj)
        {
            await _context.EventEntries.AddAsync(DtoToModel(obj));
            await _context.SaveChangesAsync();
        }

        internal async Task<EventDTO> GetById(int? id)
        {
            var recordToEdit = await _context.EventEntries.FirstOrDefaultAsync(e => e.Id == id);
            if (recordToEdit == null)
            {
                return null;
            }
            return ModelToDto(recordToEdit);
        }
        internal async Task UpdateAsync(EventDTO obj)
        {
            _context.EventEntries.Update(DtoToModel(obj));
            await _context.SaveChangesAsync();
        }

        internal async Task DeleteAsync(int id)
        {
            var recordToDelete = await _context.EventEntries.FirstOrDefaultAsync(e => e.Id == id);
            _context.Remove(recordToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
