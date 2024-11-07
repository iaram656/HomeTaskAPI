namespace appAPI.DTO
{
    public class TareaDTO
    {
        public long Id { get; set; }

        public string Description { get; set; }
        public long UserId { get; set; }
        public DateTime LimitDate { get; set; }
        public bool Status { get; set; }
        public long TareaId {  get; set; }
    }
}
