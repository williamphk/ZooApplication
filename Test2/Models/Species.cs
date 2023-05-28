using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Test2.Models
{
    public class Species
    {
        [Key]
        public int SpeciesID { get; set; }

        public string SpeciesName { get; set; }

        public bool SpeciesEndangered { get; set; }
    }
}