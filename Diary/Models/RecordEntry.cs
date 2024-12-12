using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Diary.Models
{
	public class RecordEntry
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int PhysicalState { get; set; }
		
		public string PhysicalDesc { get; set; } = string.Empty;
		[Required]
		public int MentalState { get; set; }
		
		public string MentalDesc { get; set; } = string.Empty;
		#region SkinState
		[Required]
		public int SkinState1 { get; set; }
		[Required]
		public int SkinState2 { get; set; }
		[Required]
		public int SkinState3 { get; set; }
		[Required]
		public int SkinState4 { get; set; }
		[Required]
		public int SkinState5 { get; set; }
		
		public string SkinStateDesc { get; set; } = string.Empty;
		#endregion
		[Required]
		public bool IsSportActivity {  get; set; }
		
		public string SportActivityDesc { get; set; } = string.Empty;
		[Required]
		public bool IsSexActivity { get; set; }
		[Required]
		public bool IsAlcohol { get; set; }
		[Required]
		public int Digesting {  get; set; }
		[Required]
		public int Menstruation { get; set; }
		
		public string DietDesc { get; set; } = string.Empty;

		public string? MedicationDesc { get; set; } = string.Empty;
		[Required]
		public DateTime Created { get; set; } = DateTime.Now;
		public byte[]? Photo { get; set; }

	}
}
