using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileFinder.Models
{
    public class Room
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int BuildingID { get; set; }
        public Building Building { get; set; }

        public ICollection<File> Files { get; set; }
    }
}
