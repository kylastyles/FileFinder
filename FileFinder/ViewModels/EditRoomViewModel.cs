using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FileFinder.ViewModels
{
    public class EditRoomViewModel
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Building")]
        public int BuildingID { get; set; }

        public List<SelectListItem> Buildings { get; set; }
    }
}
