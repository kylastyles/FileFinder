using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileFinder.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FileFinder.ViewModels
{
    public class CreateFileViewModel
    {
        public int Quantity { get; set; } = 1;

        public Status? Status { get; set; }

        public DateTime? ShredDate { get; set; }

        [Required]
        [Display(Name ="Consumer")]
        public int ConsumerID { get; set; }

        public List<SelectListItem> Consumers { get; set; }

        [Required]
        [Display(Name = "Case Manager")]
        public int CaseManagerID { get; set; }

        public List<SelectListItem> CaseManagers { get; set; }

        [Required]
        [Display(Name = "Location")]
        public int RoomID { get; set; }

        public List<SelectListItem> Rooms { get; set; }
    }
}
