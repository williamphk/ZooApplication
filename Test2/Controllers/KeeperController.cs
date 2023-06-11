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
    public class KeeperController : Controller
    {
        private static readonly HttpClient Client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static KeeperController()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://localhost:44360/api/");

        }
        // GET: Keeper
        public ActionResult List()
        {
            
            string url = "keeperdata/listkeepers";
            HttpResponseMessage Response = Client.GetAsync(url).Result;
            IEnumerable<KeeperDto> Keepers = Response.Content.ReadAsAsync<IEnumerable<KeeperDto>>().Result;

            return View(Keepers);
        }

        // GET: Keeper/Details/5
        public ActionResult Details(int id)
        {
            DetailsKeeper ViewModel = new DetailsKeeper();
            string url = "keeperdata/findkeeper/" + id;
            HttpResponseMessage Response = Client.GetAsync(url).Result;
            KeeperDto SelectedKeeper = Response.Content.ReadAsAsync<KeeperDto>().Result;
            ViewModel.SelectedKeeper = SelectedKeeper;

            url = "animaldata/listanimalsforkeeper/" + id;
            Response = Client.GetAsync(url).Result;
            IEnumerable<AnimalDto> KeptAnimals = Response.Content.ReadAsAsync<IEnumerable<AnimalDto>>().Result;
            ViewModel.KeptAnimals = KeptAnimals;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Keeper/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Keeper/Create
        [HttpPost]
        public ActionResult Create(Keeper keeper)
        {
            string url = "keeperdata/addkeeper";

            string jsonpayload = jss.Serialize(keeper);

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

        // GET: Keeper/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "keeperdata/findkeeper/" + id;
            HttpResponseMessage Response = Client.GetAsync(url).Result;
            KeeperDto SelectedKeeper = Response.Content.ReadAsAsync<KeeperDto>().Result;

            return View(SelectedKeeper);
        }

        // POST: Keeper/Update/5
        [HttpPost]
        public ActionResult Update(int id, Keeper keeper)
        {
            string url = "keeperdata/updatekeeper/" + id;

            string jsonpayload = jss.Serialize(keeper);

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

        // GET: Keeper/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "keeperdata/findkeeper/" + id;
            HttpResponseMessage Response = Client.GetAsync(url).Result;
            KeeperDto SelectedKeeper = Response.Content.ReadAsAsync<KeeperDto>().Result;

            return View(SelectedKeeper);
        }

        // POST: Keeper/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "keeperdata/deletekeeper/" + id;

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
