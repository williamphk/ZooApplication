using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ZooApplication.Models;
using System.Diagnostics;

namespace ZooApplication.Controllers
{
    public class AnimalDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AnimalsData/ListAnimals
        [HttpGet]
        public IEnumerable<AnimalDto> ListAnimals()
        {
            List<Animal> Animals = db.Animals.ToList();
            List<AnimalDto> AnimalDtos = new List<AnimalDto>();

            Animals.ForEach(a => AnimalDtos.Add(new AnimalDto()
            {
                AnimalID = a.AnimalID,
                AnimalName = a.AnimalName,
                AnimalWeight = a.AnimalWeight,
                SpeciesName = a.Species.SpeciesName
            }));

            return AnimalDtos;
        }

        // GET: api/AnimalsData/ListAnimalsForSpecies
        [HttpGet]
        public IEnumerable<AnimalDto> ListAnimalsForSpecies(int id)
        {
            List<Animal> Animals = db.Animals.Where(a => a.SpeciesID == id).ToList();
            List<AnimalDto> AnimalDtos = new List<AnimalDto>();

            Animals.ForEach(a => AnimalDtos.Add(new AnimalDto()
            {
                AnimalID = a.AnimalID,
                AnimalName = a.AnimalName,
                AnimalWeight = a.AnimalWeight,
                SpeciesID = a.SpeciesID,
                SpeciesName = a.Species.SpeciesName
            }));

            return AnimalDtos;
        }

        // GET: api/AnimalsData/ListAnimalsForSpecies
        [HttpGet]
        public IEnumerable<AnimalDto> ListAnimalsForKeeper(int id)
        {
            List<Animal> Animals = db.Animals.Where(a => a.Keepers.Any(k => k.KeeperID == id)).ToList();
            List<AnimalDto> AnimalDtos = new List<AnimalDto>();

            Animals.ForEach(a => AnimalDtos.Add(new AnimalDto()
            {
                AnimalID = a.AnimalID,
                AnimalName = a.AnimalName,
                AnimalWeight = a.AnimalWeight,
                SpeciesID = a.SpeciesID,
                SpeciesName = a.Species.SpeciesName
            }));

            return AnimalDtos;
        }

        [HttpPost]
        [Route("api/AnimalData/AssociateAnimalWithKeeper/{animalid}/{keeperid}")]
        public IHttpActionResult AssociateAnimalWithKeeper(int animalid, int keeperid)
        {
            Animal SelectedAnimal = db.Animals.Include(a => a.Keepers).Where(a => a.AnimalID == animalid).FirstOrDefault();
            Keeper SelectedKeeper = db.Keepers.Find(keeperid);  

            if (SelectedAnimal == null || SelectedKeeper == null) 
            {
                return NotFound();
            }
            SelectedAnimal.Keepers.Add(SelectedKeeper);
            db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("api/AnimalData/UnAssociateAnimalWithKeeper/{animalid}/{keeperid}")]
        public IHttpActionResult UnAssociateAnimalWithKeeper(int animalid, int keeperid)
        {
            Animal SelectedAnimal = db.Animals.Include(a => a.Keepers).Where(a => a.AnimalID == animalid).FirstOrDefault();
            Keeper SelectedKeeper = db.Keepers.Find(keeperid);

            if (SelectedAnimal == null || SelectedKeeper == null)
            {
                return NotFound();
            }
            SelectedAnimal.Keepers.Remove(SelectedKeeper);
            db.SaveChanges();
            return Ok();
        }

        // GET: api/AnimalsData/FindAnimal/5
        [HttpGet]
        [ResponseType(typeof(Animal))]
        public IHttpActionResult FindAnimal(int id)
        {
            Animal Animal = db.Animals.Find(id);
            AnimalDto AnimalDto = new AnimalDto()
            {
                AnimalID = Animal.AnimalID,
                AnimalName = Animal.AnimalName,
                AnimalWeight = Animal.AnimalWeight,
                SpeciesID = Animal.SpeciesID,
                SpeciesName = Animal.Species.SpeciesName
            };

            if (Animal == null)
            {
                return NotFound();
            }

            return Ok(AnimalDto);
        }

        // POST: api/AnimalsData/UpdateAnimal/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAnimal(int id, Animal animal)
        {
            Debug.WriteLine("I have reached the update method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid");
                return BadRequest(ModelState);
            }

            if (id != animal.AnimalID)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + animal.AnimalID);
                return BadRequest();
            }

            db.Entry(animal).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(id))
                {
                    Debug.WriteLine("Animal not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Debug.WriteLine("None of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AnimalsData/AddAnimal
        [ResponseType(typeof(Animal))]
        [HttpPost]
        public IHttpActionResult AddAnimal(Animal animal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Animals.Add(animal);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = animal.AnimalID }, animal);
        }

        // POST: api/AnimalsData/DeleteAnimal/5
        [ResponseType(typeof(Animal))]
        [HttpPost]
        public IHttpActionResult DeleteAnimal(int id)
        {
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return NotFound();
            }

            db.Animals.Remove(animal);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnimalExists(int id)
        {
            return db.Animals.Count(e => e.AnimalID == id) > 0;
        }
    }
}