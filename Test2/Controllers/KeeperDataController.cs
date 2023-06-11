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

namespace ZooApplication.Controllers
{
    public class KeeperDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [ResponseType(typeof(KeeperDto))]
        public IHttpActionResult ListKeepers()
        {
            List<Keeper> Keepers = db.Keepers.ToList();
            List<KeeperDto> KeeperDtos = new List<KeeperDto>();

            Keepers.ForEach(k => KeeperDtos.Add(new KeeperDto()
            {
                KeeperID = k.KeeperID,
                KeeperFirstName = k.KeeperFirstName,
                KeeperLastName = k.KeeperLastName,
            }));

            return Ok(KeeperDtos);
        }

        [HttpGet]
        [ResponseType(typeof(KeeperDto))]
        public IHttpActionResult ListKeepersForAnimal(int id)
        {
            List<Keeper> Keepers = db.Keepers.Where(k => k.Animals.Any(a => a.AnimalID == id)).ToList();
            List<KeeperDto> KeeperDtos = new List<KeeperDto>();

            Keepers.ForEach(k => KeeperDtos.Add(new KeeperDto()
            {
                KeeperID = k.KeeperID,
                KeeperFirstName = k.KeeperFirstName,
                KeeperLastName = k.KeeperLastName,
            }));

            return Ok(KeeperDtos);
        }

        [HttpGet]
        [ResponseType(typeof(KeeperDto))]
        public IHttpActionResult ListKeepersNotCaringForAnimal(int id)
        {
            List<Keeper> Keepers = db.Keepers.Where(k => !k.Animals.Any(a => a.AnimalID == id)).ToList();
            List<KeeperDto> KeeperDtos = new List<KeeperDto>();

            Keepers.ForEach(k => KeeperDtos.Add(new KeeperDto()
            {
                KeeperID = k.KeeperID,
                KeeperFirstName = k.KeeperFirstName,
                KeeperLastName = k.KeeperLastName,
            }));

            return Ok(KeeperDtos);
        }

        [ResponseType(typeof(KeeperDto))]
        [HttpGet]
        public IHttpActionResult FindKeeper(int id)
        {
            Keeper Keeper = db.Keepers.Find(id);
            KeeperDto KeeperDto = new KeeperDto()
            {
                KeeperID = Keeper.KeeperID,
                KeeperFirstName = Keeper.KeeperFirstName,
                KeeperLastName = Keeper.KeeperLastName
            };
            if (Keeper == null)
            {
                return NotFound();
            }

            return Ok(KeeperDto);
        }

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateKeeper(int id, Keeper keeper)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != keeper.KeeperID)
            {
                return BadRequest();
            }

            db.Entry(keeper).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KeeperExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(Keeper))]
        [HttpPost]
        public IHttpActionResult AddKeeper(Keeper keeper)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Keepers.Add(keeper);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = keeper.KeeperID }, keeper);
        }

        // POST: api/KeeperData/5
        [ResponseType(typeof(Keeper))]
        [HttpPost]
        public IHttpActionResult DeleteKeeper(int id)
        {
            Keeper keeper = db.Keepers.Find(id);
            if (keeper == null)
            {
                return NotFound();
            }

            db.Keepers.Remove(keeper);
            db.SaveChanges();

            return Ok(keeper);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KeeperExists(int id)
        {
            return db.Keepers.Count(e => e.KeeperID == id) > 0;
        }
    }
}