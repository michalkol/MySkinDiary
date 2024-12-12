using System.ComponentModel.DataAnnotations;

namespace Diary.Models
{
    public class DiaryEntry
    {
        [Key] // Primární klíč
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a title!")] //Vyžadovaná položka
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Minimum number of characters is 3.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a content!")]
        public string Content { get; set; } = string.Empty;
        [Required]
        public DateTime Created { get; set; } = DateTime.Now;
        


    }
}
