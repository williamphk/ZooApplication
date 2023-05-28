using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Script.Serialization;
using ZooApplication.Models;

namespace ZooApplication.Controllers
{
    public class AnimalController : Controller
    {
        private static readonly HttpClient Client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AnimalController()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://localhost:44360/api/animaldata/");

        }

        // GET: Animal
        public ActionResult List()
        {
            //Objective: communicate with our animal data api to retrieve a list of animals
            //curl: https://localhost:44360/api/animaldata/listanimals

            string url = "listanimals";
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
            //Objective: communicate with our animal data api to retrieve one animal
            //curl: https://localhost:44360/api/animaldata/findanimal/{id}

            string url = "findanimal/"+id;
            HttpResponseMessage Response = Client.GetAsync(url).Result;
            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(Response.StatusCode);
            AnimalDto SelectedAnimal = Response.Content.ReadAsAsync<AnimalDto>().Result;
            //Debug.WriteLine("Animals received: ");
            //Debug.WriteLine(SelectedAnimal.AnimalName);

            return View(SelectedAnimal);
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

        // POST: Animal/Create
        [HttpPost]
        public ActionResult Create(Animal animal)
        {
            //Debug.WriteLine("the inputted animal name is: ");
            //Debug.WriteLine(animal.AnimalName);
            //Objective: add a new animal into our system using the API
            //curl -H "Content-Type:application/json" -d @animal.json https://localhost:44360/api/animaldata/addanimal
            string url = "addanimal";

            string jsonpayload = jss.Serialize(animal);

            Debug.WriteLine("the json payload is: ");
            Debug.WriteLine(jsonpayload);

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
            return View();
        }

        // POST: Animal/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Animal/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Animal/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
