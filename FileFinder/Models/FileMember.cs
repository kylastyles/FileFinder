using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileFinder.Models
{
    public enum Role
    {
        Admin,
        User
    }

    public class FileMember : Person
    {
        public int ID { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; } = Role.Admin;
    }
}
