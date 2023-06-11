using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ZooApplication.Migrations;
using ZooApplication.Models;
using ZooApplication.Models.ViewModels;

namespace ZooApplication.Controllers
{
    public class SpeciesController : Controller
    {
        private static readonly HttpClient Client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static SpeciesController()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://localhost:44360/api/");

        }
        // GET: Species
        public ActionResult List()
        {
            string url = "speciesdata/listspecies";
            HttpResponseMessage Response = Client.GetAsync(url).Result;
            IEnumerable<SpeciesDto> Species = Response.Content.ReadAsAsync<IEnumerable<SpeciesDto>>().Result;

            return View(Species);
        }

        // GET: Species/Details/5
        public ActionResult Details(int id)
        {
            DetailsSpecies ViewModel  = new DetailsSpecies();
            string url = "speciesdata/findspecies/" + id;
            HttpResponseMessage Response = Client.GetAsync(url).Result;
            SpeciesDto SelectedSpecies = Response.Content.ReadAsAsync<SpeciesDto>().Result;
            ViewModel.SelectedSpeices = SelectedSpecies;

            url = "animaldata/listanimalsforspecies/" + id;
            Response = Client.GetAsync(url).Result;
            IEnumerable<AnimalDto> RelatedAnimals = Response.Content.ReadAsAsync<IEnumerable<AnimalDto>>().Result;
            ViewModel.RelatedAnimals = RelatedAnimals;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Animal/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Species/Create
        [HttpPost]
        public ActionResult Create(Species species)
        {
            string url = "speciesdata/addspecies";

            string jsonpayload = jss.Serialize(species);

            HttpContent httpContent = new StringContent(jsonpayload);
            httpContent.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage Response = Client.PostAsync(url, httpContent).Result;
            if (Response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Species/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "speciesdata/findspecies/" + id;
            HttpResponseMessage Response = Client.GetAsync(url).Result;
            SpeciesDto SelectedSpecies = Response.Content.ReadAsAsync<SpeciesDto>().Result;

            return View(SelectedSpecies);
        }

        // POST: Species/Update/5
        [HttpPost]
        public ActionResult Update(int id, Species species)
        {
            string url = "speciesdata/updatespecies/" + id;

            string jsonpayload = jss.Serialize(species);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage Response = Client.PostAsync(url, content).Result;
            if (Response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Species/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "speciesdata/findspecies/" + id;
            HttpResponseMessage Response = Client.GetAsync(url).Result;
            SpeciesDto SelectedSpecies = Response.Content.ReadAsAsync<SpeciesDto>().Result;

            return View(SelectedSpecies);
        }

        // POST: Species/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "speciesdata/deletespecies/" + id;


            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage Response = Client.PostAsync(url, content).Result;
            if (Response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
