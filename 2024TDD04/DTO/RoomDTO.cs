
using System.ComponentModel.DataAnnotations;

public class RoomDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name must be between 1 and 100 characters", MinimumLength = 1)]
    public required string Name { get; set; }

    [StringLength(50, ErrorMessage = "Room Abreviation must be below 50 characters", MinimumLength = 0)]
    public string? RoomAbreviation { get; set; }

    public bool IsDeleted { get; set; }
}