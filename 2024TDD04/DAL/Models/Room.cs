using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string RoomAbreviation { get; set; }
        public ICollection<Access> User_Groups { get; } = new List<Access>();
    }
}
