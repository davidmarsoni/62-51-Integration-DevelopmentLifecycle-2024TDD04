using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RoomDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must be between 1 and 100 characters", MinimumLength = 1)]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Room Abreviation is required")]
        [StringLength(100, ErrorMessage = "Room Abreviation must be between 1 and 100 characters", MinimumLength = 1)]
        public required string RoomAbreviation { get; set; }
    }
}
