using System.ComponentModel.DataAnnotations;

namespace Diary.DTO
{
    public class RecordDtoApi
    {
        public int Id { get; set; }
        
        public int PhysicalState { get; set; }

        //public string PhysicalDesc { get; set; } = string.Empty;

        public int MentalState { get; set; }

        //public string MentalDesc { get; set; } = string.Empty;
        //#region SkinState

        //public int SkinState1 { get; set; }

        //public int SkinState2 { get; set; }

        //public int SkinState3 { get; set; }

        //public int SkinState4 { get; set; }

        //public int SkinState5 { get; set; }

        //public string SkinStateDesc { get; set; } = string.Empty;
        //#endregion

        //public bool IsSportActivity { get; set; }
        //public string SportActivityDesc { get; set; } = string.Empty;

        //public bool IsSexActivity { get; set; }

        //public bool IsAlcohol { get; set; }

        //public int Digesting { get; set; }

        //public int Menstruation { get; set; }

        //public string DietDesc { get; set; } = string.Empty;
        //public string MedicationDesc { get; set; } = string.Empty;

        public DateTime Created { get; set; } = DateTime.Now;
        //public byte[]? Photo { get; set; }
    }
}
