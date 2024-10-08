using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public required string Name { get; set; }
        public string? Acronym { get; set; } = null!;

        public ICollection<User> Users { get; } = new List<User>();

        public ICollection<UserGroup> User_Groups { get; } = new List<UserGroup>();
    }
}
