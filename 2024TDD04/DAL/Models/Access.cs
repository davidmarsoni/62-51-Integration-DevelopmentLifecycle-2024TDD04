using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Access
    {
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;
        public AccessType AccessType { get; set; }
    }
}
