using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZooApplication.Models.ViewModels
{
    public class UpdateAnimal
    {
        public AnimalDto SelectedAnimal { get; set; }

        public IEnumerable<SpeciesDto> SpeciesOptions { get; set; }
    }
}