using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileFinder.Data;

namespace FileFinder.Models
{
    public class Audit
    {
        public int ID { get; set; }

        public string FiscalYear { get; set; }

        public IEnumerable<File> FirstQuarter { get; set; }

        public IEnumerable<File> SecondQuarter { get; set; }

        public IEnumerable<File> ThirdQuarter { get; set; }

        public IEnumerable<File> FourthQuarter { get; set; }


        //public Audit()
        //{
        //    List<File> FirstQuarter = new List<File>();
        //    List<File> SecondQuarter = new List<File>();
        //    List<File> ThirdQuarter = new List<File>();
        //    List<File> FourthQuarter = new List<File>();
        //}


        public List<List<File>> SortFiles(IEnumerable<File> allFiles)
        {
            List<File> group1 = new List<File>();
            List<File> group2 = new List<File>();
            List<File> group3 = new List<File>();
            List<File> group4 = new List<File>();

            int counter = 1;

            foreach (File x in allFiles)
            {
                switch(counter)
                {
                    case 1:
                        group1.Add(x); break;
                    case 2:
                        group2.Add(x); break;
                    case 3:
                        group3.Add(x); break;
                    case 4:
                        group4.Add(x); break;
                }

                counter++;
                if (counter > 4)
                {
                    counter = 1;
                }                
            }

            List<List<File>> allGroups = new List<List<File>>();
            allGroups.Add(group1);
            allGroups.Add(group2);
            allGroups.Add(group3);
            allGroups.Add(group4);

            return allGroups;
        }
    }
}
