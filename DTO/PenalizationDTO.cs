namespace appAPI.DTO
{
    public class PenalizationDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Reason { get; set;}

        public DateTime Date { get; set; }
        public long Points { get; set; }

        public long PenId { get; set; }
    }
}
