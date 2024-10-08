using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string? Name { get; set; }

        public Access Access { get; set; } = null!;
    }
}
