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

    public class User : Person
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; } = Role.Admin;
    }
}
