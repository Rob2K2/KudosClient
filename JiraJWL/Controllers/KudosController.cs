using JiraJWL.Modelos;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace JiraJWL.Controllers
{
    public class KudosController : Controller
    {
        // GET: Kudos
        public ActionResult Index()
        {
            var kudos = new List<Kudos>();
            kudos = GetKudos();

            return View(kudos);
        }

        // GET: Kudos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Kudos/Create
        public ActionResult Create()
        {
            var users = GetUsers();
            ViewBag.Users = users;

            return View();
        }

        // POST: Kudos/Create
        [HttpPost]
        public ActionResult Create(Kudos kudos, string fuente, string destino)
        {
            kudos.Fecha = DateTime.Now.ToShortDateString();

            var users = GetUsers();
            ViewBag.Users = users;

            if (ModelState.IsValid)
            {
                CreateKudos(kudos);

                return RedirectToAction("Index");
            }

            return View("Create", kudos);
        }

        // GET: Kudos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Kudos/Edit/5
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

        // GET: Kudos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Kudos/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, string destino)
        {
            var client = new RestClient("http://localhost:61798/api/kudos/" + id);
            var request = new RestRequest(Method.DELETE);

            string json = "{\"destino\":\"" + destino + "\"}";

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            var response = client.Execute(request);

            return RedirectToAction("Index");
        }

        private List<Kudos> GetKudos()
        {
            var client = new RestClient("http://localhost:61798/api/kudos");
            var request = new RestRequest(Method.GET);

            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Kudos>>(request);

            return response.Data;
        }

        private void CreateKudos(Kudos kudos)
        {
            var client = new RestClient("http://localhost:61798/api/kudos");
            var request = new RestRequest(Method.POST);

            string json = "{\"fuente\":\"" + kudos.Fuente + "\", \"destino\":\"" + "" + kudos.Destino + "\",\"tema\":\"" + kudos.Tema + "\",\"fecha\":\"" + kudos.Fecha + "\",\"lugar\":\"" + kudos.Lugar + "\",\"texto\":\"" + kudos.Texto + "\"}";

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            var response = client.Execute(request);
        }

        private List<User> GetUsers()
        {
            var client = new RestClient("http://localhost:58443/api/user");
            var request = new RestRequest(Method.GET);

            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<User>>(request);

            return response.Data;
        }
    }
}
