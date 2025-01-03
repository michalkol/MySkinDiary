namespace Diary.ViewModel
{
	public class UserProfileViewModel
	{
		public string Id { get; set; } // ID uživatele
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public byte[]? Photo { get; set; } // Profilová fotka
		public IFormFile? PhotoFile { get; set; } // Soubor nahrávané fotky
	}
}
