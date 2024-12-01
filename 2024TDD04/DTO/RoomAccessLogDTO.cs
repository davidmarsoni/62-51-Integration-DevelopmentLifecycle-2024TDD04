using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RoomAccessLogDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int? GroupId { get; set; }
        public int UserId { get; set; }
        public string AccessType { get; set; }
        public string Info { get; set; }
    }
}
