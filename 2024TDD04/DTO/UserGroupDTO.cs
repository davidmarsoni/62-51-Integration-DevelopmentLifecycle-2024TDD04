using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class UserGroupDTO
    {

        public int Id { get; set; }
        
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }
        public string? Username { get; set; }

        [Required(ErrorMessage = "Group ID is required")]

        public int GroupId { get; set; }

        public string? Groupname { get; set; }
    }
}
