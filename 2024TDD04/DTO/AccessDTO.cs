using DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AccessDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Room ID is required")]
        public required int RoomId { get; set; }
        [Required(ErrorMessage = "Group ID is required")]
        public required int GroupId { get; set; }
        public required string AccessType { get; set; }
    }
}
