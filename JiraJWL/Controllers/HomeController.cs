using JiraJWL.Modelos;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JiraJWL.Controllers
{
    public class HomeController : Controller
    {
        List<User> users = new List<User>();

        public ActionResult Index()
        {
            //List<User> users = new List<User>();
            users = GetUsers();
            //ViewBag.Projects = projects;

            return View(users);
        }

        //[HttpPost]
        //public ActionResult Index(string project)
        //{

        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private List<User> GetUsers()
        {
            User user1 = new User
            {
                UserID = 1,
                FirstName = "Roberto",
                LastName = "Merino",
                NickName = "RVD",
                TotalKudos = 0
            };

            User user2 = new User
            {
                UserID = 2,
                FirstName = "Elena",
                LastName = "Arce",
                NickName = "Helen",
                TotalKudos = 0
            };

            User user3 = new User
            {
                UserID = 3,
                FirstName = "Diego",
                LastName = "Bellido",
                NickName = "Belli",
                TotalKudos = 0
            };

            users.Add(user1);
            users.Add(user2);
            users.Add(user3);

            return users;
        }

        private List<Issue> GetIssues(string project)
        {
            string name = Session["name"].ToString();
            string value = Session["value"].ToString();
            var client = new RestClient("https://bcomunidad.atlassian.net/rest/api/2/search?jql=project=" + project);
            var request = new RestRequest(Method.GET);
            request.AddCookie(name, value);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<IssueList>>(request);

            return response.Data[0].Issues;
        }

        [HttpPost]
        public ActionResult Issue(string issueKey)
        {
            return View(GetIssue(issueKey));
        }

        [HttpPost]
        public ActionResult AddWorklog(string issueKey, string timeSpent, string timeOriginalEstimate, string seconds)
        {
            string name = Session["name"].ToString();
            string value = Session["value"].ToString();
            bool res = false;

            long lngTimeSpent = 0;
            res = long.TryParse(timeSpent, out lngTimeSpent);
            if (!res)
            {
                lngTimeSpent = 0;
            }

            long lngTimeOriginalEstimate = long.Parse(timeOriginalEstimate);
            long segundos = long.Parse(seconds);

            if (segundos + lngTimeSpent >= lngTimeOriginalEstimate + lngTimeOriginalEstimate * 0.1)
            {
                TempData["alert"] = true;
            }

            string fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff-0400");

            var client = new RestClient("https://bcomunidad.atlassian.net/rest/api/2/issue/" + issueKey + "/worklog");
            var request = new RestRequest(Method.POST);
            request.AddCookie(name, value);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n\"comment\": {\r\n    \"type\": \"doc\",\r\n    \"version\": 1,\r\n    \"content\": [\r\n      {\r\n        \"type\": \"paragraph\",\r\n        \"content\": [\r\n          {\r\n            \"type\": \"text\",\r\n            \"text\": \"Increasing time\"\r\n          }\r\n        ]\r\n      }\r\n    ]\r\n  },\r\n  \"started\": \"" + fecha + "\",\r\n  \"timeSpentSeconds\": " + segundos + "\r\n}",
                ParameterType.RequestBody);
            var response = client.Execute(request);

            if (response.StatusCode.ToString() == "Created")
            {
                TempData["successful"] = issueKey;
            }

            //return RedirectToAction("Index");
            return View("Issue", GetIssue(issueKey));
        }

        private Issue GetIssue(string issueKey)
        {
            //string name = Session["name"].ToString();
            //string value = Session["value"].ToString();

            var client = new RestClient("https://bcomunidad.atlassian.net/rest/api/2/issue/" + issueKey);
            var request = new RestRequest(Method.GET);
            //request.AddCookie(name, value);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<Issue>(request);

            return response.Data;
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Kudos/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                CreateUser(user);

                return RedirectToAction("Index");
            }

            return View("Create", user);
        }

        private void CreateUser(User user)
        {
            var client = new RestClient("http://localhost:58443/api/user");
            var request = new RestRequest(Method.POST);

            string json = "{\"firstName\":\"" + user.FirstName + "\", \"lastName\":\"" + "" + user.LastName + "\",\"NickName\":\"" + user.NickName + "\"}";

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            var response = client.Execute(request);
        }

        public bool EsValido(User user)
        {
            var valido = true;

            if (user.Edad > 200)
            {
                valido = false;
            }

            if (user.Edad < 0)
            {
                valido = false;
            }

            return valido;
        }
    }
}