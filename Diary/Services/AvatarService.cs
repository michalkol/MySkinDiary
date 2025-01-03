using Diary.Data;
using Diary.DTO;
using Diary.Models;
using Microsoft.EntityFrameworkCore;

namespace Diary.Services
{
	public class AvatarService
	{
		private ApplicationDbContext _context;

		public AvatarService(ApplicationDbContext context)
		{
			_context = context;
		}

		internal IEnumerable<AvatarDTO> GetAllRecords()
		{
			var allRecords = _context.Avatars;

			var allRecordsDtos = new List<AvatarDTO>();
			foreach (var item in allRecords)
			{
				allRecordsDtos.Add(ModelToDto(item));
			}
			return allRecordsDtos;
		}

		internal IQueryable<AvatarDTO> searchService()
		{
			return _context.Avatars.Select(item => new AvatarDTO
			{
				// Můžete zde nastavit vlastnosti DTO
				Id = item.Id,
				UserId = item.UserId,
				Photo = item.Photo,
			});
		}

		public AvatarDTO ModelToDto(AvatarPhoto item)
		{
			return new AvatarDTO
			{
				Id = item.Id,
				UserId = item.UserId,
				Photo = item.Photo,
			};
		}
		public AvatarPhoto DtoToModel(AvatarDTO item)
		{
			return new AvatarPhoto
			{
				Id = item.Id,
				UserId = item.UserId,
				Photo = item.Photo,
			};
		}

		internal async Task CreateRecordAsync(AvatarDTO obj)
		{
			await _context.Avatars.AddAsync(DtoToModel(obj));
			await _context.SaveChangesAsync();
		}

		internal async Task<AvatarDTO> GetById(string id)
		{
			var recordToEdit = await _context.Avatars.FirstOrDefaultAsync(rec => rec.UserId == id);
			if (recordToEdit == null)
			{
				return null;
			}
			return ModelToDto(recordToEdit);
		}

		internal async Task UpdateAsync(AvatarDTO obj)
		{
			_context.Avatars.Update(DtoToModel(obj));
			await _context.SaveChangesAsync();
		}

		internal async Task DeleteAsync(int id)
		{
			var recordToDelete = await _context.Avatars.FirstOrDefaultAsync(x => x.Id == id);
			_context.Remove(recordToDelete);
			await _context.SaveChangesAsync();
		}
	}
}
