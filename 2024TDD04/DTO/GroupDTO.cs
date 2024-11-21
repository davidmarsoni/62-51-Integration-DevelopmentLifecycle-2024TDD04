using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class GroupDTO
    {
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must be between 1 and 100 characters", MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(10, ErrorMessage = "Acronym must be between 1 and 10 characters", MinimumLength = 1)]
        public string? Acronym { get; set; }

        [Required(ErrorMessage = "IsDeleted is required")]
        public bool IsDeleted { get; set; }

        public string DisplayName => $"{Name} ({Acronym})";
    }
}