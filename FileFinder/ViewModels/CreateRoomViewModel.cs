using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FileFinder.ViewModels
{
    public class CreateRoomViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Building")]
        public int BuildingID { get; set; }

        public List<SelectListItem> Buildings { get; set; }
    }
}
