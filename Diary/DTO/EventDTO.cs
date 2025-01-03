namespace Diary.DTO
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Description { get; set; }
        public string Location { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public byte[]? Photo { get; set; }
    }
}
