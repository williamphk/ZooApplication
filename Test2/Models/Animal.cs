using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test2.Models
{
    public class Animal
    {
        [Key]
        public int AnimalID { get; set; }
        public string AnimalName { get; set; }

        //weight is in kg
        public int AnimalWeight { get; set; }

        //An animal belongs to one species
        //A species can have many animals
        [ForeignKey("Species")]
        public int SpeciesID { get; set; }
        public virtual Species Species { get; set; }

        //An animal can be taken care of many keepers

        public ICollection<Keeper> Keepers { get; set; }

    }

    public class AnimalDto
    {
        public int AnimalID { get; set; }
        public string AnimalName { get; set; }

        //weight is in kg
        public int AnimalWeight { get; set; }

        public string SpeciesName { get; set; }
    }
}