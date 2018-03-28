using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileFinder.Models
{
    public class Consumer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public bool Active { get; set; }

        public IList<File> Files { get; set; }

        public Consumer()
        {
            Active = true; //initialize new consumers as "Active"
        }
    }
}
