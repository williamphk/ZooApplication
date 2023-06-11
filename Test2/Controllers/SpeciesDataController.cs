using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ZooApplication.Migrations;
using ZooApplication.Models;

namespace ZooApplication.Controllers
{
    public class SpeciesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SpeciesData/ListSpecies
        [HttpGet]
        public IEnumerable<SpeciesDto> ListSpecies()
        {
            List<Species> Species = db.Species.ToList();
            List<SpeciesDto> SpeciesDtos = new List<SpeciesDto>();

            Species.ForEach(s => SpeciesDtos.Add(new SpeciesDto()
            {
                SpeciesID = s.SpeciesID,
                SpeciesName = s.SpeciesName,
                SpeciesEndangered = s.SpeciesEndangered,
            }));

            return SpeciesDtos;
        }

        // GET: api/SpeciesData/FindSpecies/5
        [HttpGet]
        [ResponseType(typeof(Species))]
        public IHttpActionResult FindSpecies(int id)
        {
            Species Species = db.Species.Find(id);
            SpeciesDto SpeciesDto = new SpeciesDto()
            {
                SpeciesID = Species.SpeciesID,
                SpeciesName = Species.SpeciesName,
                SpeciesEndangered = Species.SpeciesEndangered,
            };

            if (Species == null)
            {
                return NotFound();
            }

            return Ok(SpeciesDto);
        }

        // POST: api/SpeciesData/UpdateSpecies/5
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateSpecies(int id, Species species)
        {
            Debug.WriteLine("I have reached the update method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid");
                return BadRequest(ModelState);
            }

            if (id != species.SpeciesID)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + species.SpeciesID);
                return BadRequest();
            }

            db.Entry(species).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpeciesExists(id))
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

        // POST: api/SpeciesData/AddSpecies
        [HttpPost]
        [ResponseType(typeof(Species))]
        public IHttpActionResult AddSpecies(Species species)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Species.Add(species);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = species.SpeciesID }, species);
        }

        // POST: api/SpeciesData/DeleteSpecies/5
        [HttpPost]
        [ResponseType(typeof(Species))]
        public IHttpActionResult DeleteSpecies(int id)
        {
            Species species = db.Species.Find(id);
            if (species == null)
            {
                return NotFound();
            }

            db.Species.Remove(species);
            db.SaveChanges();

            return Ok(species);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpeciesExists(int id)
        {
            return db.Species.Count(e => e.SpeciesID == id) > 0;
        }
    }
}