using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileFinder.Models
{
    public class File
    {
        public int ConsumerID { get; set; }
        public Consumer Consumer { get; set; }

        public int CaseManagerID { get; set; }
        public CaseManager CaseManager { get; set; }

        public int LocationID { get; set; }
        public Location Location { get; set; }

        public DateTime ShredDate { get; set; }

        public File()
        {

        }
    }
}
