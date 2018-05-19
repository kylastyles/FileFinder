using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FileFinder.Models;

namespace FileFinder.ViewModels
{
    public class CreateBuildingViewModel
    {
        public string Name { get; set; }

        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name="Phone Number")]
        public string PhoneNumber { get; set; }

        public List<Room> Rooms { get; set; }


    }
}
