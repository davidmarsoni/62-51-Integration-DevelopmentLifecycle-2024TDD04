using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    class RoomAccessLog
    {
        public int logId { get; set; }
        public int userId { get; set; }
        public User? User { get; set; } = null!;
        public int? groupId { get; set; }
        public Group? Group { get; set; } = null!;
        public int roomId { get; set; }
        public Room? Room { get; set; } = null!;
        public AccessType accessType { get; set; }
        public required string info { get; set; }
        public DateTime timestamp { get; set; }
    }
}
