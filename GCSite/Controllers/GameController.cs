using GCSite.Data;
using GCSite.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using X.PagedList.Mvc;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace GCSite.Controllers
{
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GameController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(string currentFilter, string searchString, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            IQueryable<Game> objList;
            if (!String.IsNullOrEmpty(searchString))            
                objList = _db.Games.Where(s => s.GameName.Contains(searchString));
            else
                 objList = _db.Games;

            int pageSize = 10;
            TempData["localPath"] = Helpers.PathAddBackslash(Path.Combine(_webHostEnvironment.WebRootPath, "image", "upload", "t_thumb"));
            return View(await PaginatedList<Game>.CreateAsync(objList.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Search(string searchString)
        {
            List<GameSearchViewModel> objList = new List<GameSearchViewModel>();
            if (searchString != "" && searchString != null)
            {
                var client = new RestClient("https://api.igdb.com/v4/games");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Client-ID", "4bpvax0sj2c2bdnkfbnw8wup0rzspy");
                request.AddHeader("Authorization", "Bearer "+ Environment.GetEnvironmentVariable("IGDBKEY", EnvironmentVariableTarget.User));
                request.AddHeader("Content-Type", "text/plain");
                request.AddParameter("text/plain", $"fields name, cover.*,first_release_date,genres.name,platforms.name,involved_companies.*,involved_companies.company.name; where name ~ *\"{searchString}\"*; limit 50;", ParameterType.RequestBody);
                IRestResponse response = await client.ExecuteAsync(request);

                List<GameRoot> convertedResponse = JsonConvert.DeserializeObject<List<GameRoot>>(response.Content);


                GameSearchViewModel tempGame = new GameSearchViewModel();

                foreach (GameRoot g in convertedResponse)
                {
                    if (g.genres != null
                        && g.cover != null
                        && g.involved_companies != null
                        && g.platforms != null)
                    {
                        tempGame.GameName = g.name;
                        tempGame.IGDBId = g.id;
                        foreach (InvolvedCompany c in g.involved_companies)
                        {
                            if (c.developer)
                            {
                                if (tempGame.Developer == null)
                                {
                                    tempGame.Developer = c.company.name;
                                }
                            }

                            if (c.publisher)
                            {
                                if (tempGame.Publisher == null)
                                {
                                    tempGame.Publisher = c.company.name;
                                }
                            }
                        }
                        tempGame.Platform = g.platforms[0].name;
                        tempGame.Genre = g.genres[0].name;
                        tempGame.ReleaseDateUs = IGDBAPIHelper.UnixTimeStampConverter(g.first_release_date);
                        tempGame.ThumbnailPath = g.cover.url;
                        objList.Add(tempGame);
                        tempGame = new GameSearchViewModel();
                    }
                    else if (g.name != null)
                    {
                        tempGame.GameName = g.name;
                    }
                }

                List<Task> downloadList = new List<Task>();
                foreach (GameSearchViewModel gsModel in objList)
                {
                    Game temp = new Game();
                    temp.IGDBId = gsModel.IGDBId;
                    temp.GameName = gsModel.GameName;
                    temp.Developer = gsModel.Developer;
                    temp.Publisher = gsModel.Publisher;
                    temp.Platform = gsModel.Platform;
                    temp.Genre = gsModel.Genre;
                    temp.ReleaseDateUs = gsModel.ReleaseDateUs;
                    temp.ThumbnailPath = gsModel.ThumbnailPath;
                    if (!_db.Games.Any(x => x.IGDBId == temp.IGDBId))
                    {
                        await _db.AddAsync(temp);
                        string[] splitString = temp.ThumbnailPath.Split('/');
                        string imgUrl = @"/image/upload/t_thumb/" + splitString[splitString.Length - 1];
                        string localFileName = Helpers.PathAddBackslash(Path.Combine(_webHostEnvironment.WebRootPath, "image", "upload", "t_thumb")) + splitString[splitString.Length - 1];
                        if (!System.IO.File.Exists(localFileName))
                        {
                            using (WebClient wc = new WebClient())
                            {
                                downloadList.Add(wc.DownloadFileTaskAsync("https:" + temp.ThumbnailPath, localFileName));
                            }
                        }
                    }
                }
                //download all the new thumbnails at once
                foreach (Task t in downloadList)
                {
                    await t;
                }
                await _db.SaveChangesAsync();
            }
            TempData["localPath"] = Helpers.PathAddBackslash(Path.Combine(_webHostEnvironment.WebRootPath, "image", "upload", "t_thumb"));
            return View(objList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Game g)
        {
            _db.Add(g);
            _db.SaveChanges();
            TempData["localPath"] = Helpers.PathAddBackslash(Path.Combine(_webHostEnvironment.WebRootPath, "image", "upload", "t_thumb"));
            return RedirectToAction("Index");
        }
    }
}
