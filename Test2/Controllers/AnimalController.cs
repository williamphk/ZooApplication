using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Script.Serialization;
using ZooApplication.Migrations;
using ZooApplication.Models;
using ZooApplication.Models.ViewModels;

namespace ZooApplication.Controllers
{
    public class AnimalController : Controller
    {
        private static readonly HttpClient Client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AnimalController()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://localhost:44360/api/");

        }

        // GET: Animal
        public ActionResult List()
        {
            //Objective: communicate with our animal data api to retrieve a list of animals
            //curl: https://localhost:44360/api/animaldata/listanimals

            string url = "animaldata/listanimals";
            HttpResponseMessage Response = Client.GetAsync(url).Result;
            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(Response.StatusCode);
            IEnumerable<AnimalDto> Animals = Response.Content.ReadAsAsync<IEnumerable<AnimalDto>>().Result;
            //Debug.WriteLine("Number of animals received: ");
            //Debug.WriteLine(Animals.Count());

            return View(Animals);
        }

        // GET: Animal/Details/5
        public ActionResult Details(int id)
        {
            DetailsAnimal ViewModel = new DetailsAnimal();
            //Objective: communicate with our animal data api to retrieve one animal
            //curl: https://localhost:44360/api/animaldata/findanimal/{id}

            string url = "animaldata/findanimal/" + id;
            HttpResponseMessage Response = Client.GetAsync(url).Result;
            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(Response.StatusCode);
            AnimalDto SelectedAnimal = Response.Content.ReadAsAsync<AnimalDto>().Result;
            //Debug.WriteLine("Animals received: ");
            //Debug.WriteLine(SelectedAnimal.AnimalName);
            ViewModel.SelectedAnimal = SelectedAnimal;

            url = "keeperdata/listkeepersforanimal/" + id;
            Response = Client.GetAsync(url).Result;
            IEnumerable<KeeperDto> ResponsibleKeepers = Response.Content.ReadAsAsync<IEnumerable<KeeperDto>>().Result;
            ViewModel.ResponsibleKeepers = ResponsibleKeepers;

            url = "keeperdata/listkeepersnotcaringforanimal/" + id;
            Response = Client.GetAsync(url).Result;
            IEnumerable<KeeperDto> AvailableKeepers = Response.Content.ReadAsAsync<IEnumerable<KeeperDto>>().Result;
            ViewModel.AvailableKeepers = AvailableKeepers;


            return View(ViewModel);
        }

        //POST: Animal/Associate/{animalid}
        [HttpPost]
        public ActionResult Associate(int id, int KeeperID)
        {
            string url = "animaldata/associateanimalwithkeeper/" + id + "/" + KeeperID;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage Response = Client.PostAsync(url, content).Result;
            return RedirectToAction("Details/" + id);
        }

        //GET: Animal/UnAssociate/{animalid}
        [HttpGet]
        public ActionResult UnAssociate(int id, int KeeperID)
        {
            string url = "animaldata/unassociateanimalwithkeeper/" + id + "/" + KeeperID;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage Response = Client.PostAsync(url, content).Result;
            return RedirectToAction("Details/" + id);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Animal/New
        public ActionResult New()
        {
            string url = "speciesdata/listspecies";
            HttpResponseMessage Response = Client.GetAsync(url).Result;
            IEnumerable<SpeciesDto> SpeciesOptions = Response.Content.ReadAsAsync<IEnumerable<SpeciesDto>>().Result;
            return View(SpeciesOptions);
        }

        // POST: Animal/Create
        [HttpPost]
        public ActionResult Create(Animal animal)
        {
            //Debug.WriteLine("the inputted animal name is: ");
            //Debug.WriteLine(animal.AnimalName);
            //Objective: add a new animal into our system using the API
            //curl -H "Content-Type:application/json" -d @animal.json https://localhost:44360/api/animaldata/addanimal
            string url = "animaldata/addanimal";

            string jsonpayload = jss.Serialize(animal);

            //Debug.WriteLine("the json payload is: ");
            //Debug.WriteLine(jsonpayload);

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

        // GET: Animal/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateAnimal ViewModel = new UpdateAnimal();
            string url = "animaldata/findanimal/" + id;
            HttpResponseMessage Response = Client.GetAsync(url).Result;
            AnimalDto SelectedAnimal = Response.Content.ReadAsAsync<AnimalDto>().Result;
            ViewModel.SelectedAnimal = SelectedAnimal;

            url = "speciesdata/listspecies";
            Response = Client.GetAsync(url).Result;
            IEnumerable<SpeciesDto> SpeciesOptions = Response.Content.ReadAsAsync<IEnumerable<SpeciesDto>>().Result;
            ViewModel.SpeciesOptions = SpeciesOptions;

            return View(ViewModel);
        }

        // POST: Animal/Update/5
        [HttpPost]
        public ActionResult Update(int id, Animal animal)
        {
            string url = "animaldata/updateanimal/" + id;

            string jsonpayload = jss.Serialize(animal);

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

        // GET: Animal/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "animaldata/findanimal/" + id;
            HttpResponseMessage Response = Client.GetAsync(url).Result;
            AnimalDto SelectedAnimal = Response.Content.ReadAsAsync<AnimalDto>().Result;

            return View(SelectedAnimal);
        }

        // POST: Animal/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "animaldata/deleteanimal/" + id;


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
