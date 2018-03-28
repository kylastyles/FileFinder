using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileFinder.Models
{
    public class Program
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IList<CaseManager> CaseManagers { get; set; }

        public Program()
        {

        }
    }
}
