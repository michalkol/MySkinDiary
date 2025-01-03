using Diary.Migrations;
using Diary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Diary.DTO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Diary.Services;

namespace Diary.Data
{
	// Rozšíření dědičné třídy DbContext z EntityFramework
	public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
		private readonly ICurrentUserService _currentUserService;
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options) 
        {
			_currentUserService = currentUserService;
		}

		//vytvoření nové tabulky databáze
		public DbSet<DiaryEntry>  DiaryEntries { get; set; }
		public DbSet<RecordEntry> RecordEntries { get; set; }
		public DbSet<EventEntry> EventEntries { get; set; }
		public DbSet<AvatarPhoto> Avatars { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Globální filtr
			modelBuilder.Entity<RecordEntry>()
				.HasQueryFilter(record => record.UserId == _currentUserService.UserId);
		}


	}
}

// Nástroje/Správa NuGet/Konzola -> add-migration AddDiaryEntryTable (vytvoří změnu v databázi přidáním nové tabulky), vytvoří také složku Migrations se soubory

// Potvrdit změny commandem update-database
//add-migration AddRecordEntryTable