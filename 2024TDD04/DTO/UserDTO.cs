using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required (ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "IsDeleted is required")]
        public bool IsDeleted { get; set; }
    }
}