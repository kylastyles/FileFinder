using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileFinder.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Building { get; set; }
        public string Room { get; set; }
        public string Cabinet { get; set; }

        public IList<File> Files { get; set; }

        public Location()
        {
            
        }
    }
}
