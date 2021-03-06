﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileFinder.Models
{
    public class Program
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        [Display(Name="Case Managers")]
        public ICollection<CaseManager> CaseManagers { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
