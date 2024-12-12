using Diary.Migrations;
using Diary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Diary.DTO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Diary.Data
{
	// Rozšíření dědičné třídy DbContext z EntityFramework
	public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }

		//vytvoření nové tabulky databáze
		public DbSet<DiaryEntry>  DiaryEntries { get; set; }
		public DbSet<RecordEntry> RecordEntries { get; set; }
		
	}
}

// Nástroje/Správa NuGet/Konzola -> add-migration AddDiaryEntryTable (vytvoří změnu v databázi přidáním nové tabulky), vytvoří také složku Migrations se soubory

// Potvrdit změny commandem update-database
//add-migration AddRecordEntryTable