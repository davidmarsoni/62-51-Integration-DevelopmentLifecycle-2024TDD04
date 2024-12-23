using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class RoomAccessLog
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; } = null!;
        public int? GroupId { get; set; }
        public Group? Group { get; set; } = null!;
        public int RoomId { get; set; }
        public string Room { get; set; }
        public required string Info { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
