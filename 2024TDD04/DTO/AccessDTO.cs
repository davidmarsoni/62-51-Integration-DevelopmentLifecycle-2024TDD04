using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class AccessDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Room ID is required")]
        public required int RoomId { get; set; }
        [Required(ErrorMessage = "Group ID is required")]
        public required int GroupId { get; set; }
    }
}
