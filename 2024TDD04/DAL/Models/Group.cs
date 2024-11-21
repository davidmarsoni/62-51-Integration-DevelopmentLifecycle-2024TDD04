using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Acronym { get; set; }
        public bool IsDeleted { get; set; } = false!;

        public ICollection<User> Users { get; } = new List<User>();
        public ICollection<UserGroup> User_Groups { get; } = new List<UserGroup>();
    }
}
