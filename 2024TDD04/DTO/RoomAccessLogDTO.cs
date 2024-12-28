namespace DTO
{
    public class RoomAccessLogDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public String? RoomName { get; set; }
        public int UserId { get; set; }
        public String? Username { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string Info { get; set; }
    }
}
