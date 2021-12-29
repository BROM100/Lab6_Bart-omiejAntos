using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration; //IConfiguration
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MoviesApi.Models;


namespace WebMvc.Controllers
{
    public class ClientController : Controller
    {
        private readonly HttpClient client;
        private readonly string WebApiPath;
        private readonly IConfiguration _configuration;

        public ClientController(IConfiguration configuration)
        {
            _configuration = configuration;
            WebApiPath = _configuration["MoviesApiConfig:Url"];   //read from appsettings.json
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", _configuration["MoviesApiConfig:ApiKey"]);   //use on any http calls      
        }


        // GET: ClientController
        public async Task<ActionResult> Index()
        {
            List<MoviesItem> todos = null;
            HttpResponseMessage response = await client.GetAsync(WebApiPath);
            if (response.IsSuccessStatusCode)
            {
                todos = await response.Content.ReadAsAsync<List<MoviesItem>>();  //requires System.Net.Http.Formatting.Extension
            }
            return View(todos);
        }



        // GET: ClientController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + id);
            if (response.IsSuccessStatusCode)
            {
                MoviesItem movies = await response.Content.ReadAsAsync<MoviesItem>();
                return View(movies);
            }
            return NotFound();
        }


        // GET: ClientController/Create
        public ActionResult Create()
        {
            return View();
        }



        // POST: ClientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]  //movies add deoc to slides
        public async Task<ActionResult> Create([Bind("Id,Name,IsWatched")] MoviesItem movies)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(WebApiPath, movies);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }



        // GET: ClientController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + id);
            if (response.IsSuccessStatusCode)
            {
                MoviesItem movies = await response.Content.ReadAsAsync<MoviesItem>();
                return View(movies);
            }
            return NotFound();
        }


        // POST: ClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Name,IsWatched")] MoviesItem movies)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(WebApiPath + id, movies);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            return View(movies);
        }




        // GET: ClientController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + id);
            if (response.IsSuccessStatusCode)
            {
                MoviesItem movies = await response.Content.ReadAsAsync<MoviesItem>();
                return View(movies);
            }
            return NotFound();
        }


        // POST: ClientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, int notUsed = 0)
        {
            HttpResponseMessage response = await client.DeleteAsync(WebApiPath + id);
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }
    }
}

