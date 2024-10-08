using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class User
    {
        public int UserId { get; set; }
        public required string Username { get; set; }

        public ICollection<Group> Groups { get; } = new List<Group>();
        public ICollection<UserGroup> User_Groups { get; } = new List<UserGroup>();
    }
}