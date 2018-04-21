using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileFinder.Models
{
    public class Building
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}
