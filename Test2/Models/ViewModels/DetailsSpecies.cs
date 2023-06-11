using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZooApplication.Models.ViewModels
{
    public class DetailsSpecies
    {
        public SpeciesDto SelectedSpeices { get; set; }
        public IEnumerable<AnimalDto> RelatedAnimals { get; set; }
    }
}