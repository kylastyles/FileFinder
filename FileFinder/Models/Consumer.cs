using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FileFinder.Models
{
    public class Consumer : Person
    {
        public int ID { get; set; }

        [Display(Name ="Date of Birth")]
        [DisplayFormat(DataFormatString ="{0:mm/dd/yyyy}")]
        public DateTime DOB { get; set; }

        public bool Active { get; set; } = true;


        public ICollection<File> Files { get; set; }


    }
}
