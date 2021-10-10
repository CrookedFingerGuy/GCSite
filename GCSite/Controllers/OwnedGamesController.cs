using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GCSite.Data;
using GCSite.Models;
using System.Security.Claims;
using RestSharp;
using System.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using X.PagedList.Mvc;
using X.PagedList;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace GCSite.Controllers
{
    public class OwnedGamesController : Controller
    {
        private readonly ApplicationDbContext _context;        

        public OwnedGamesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Testing()
        {
            return View();
        }

        // GET: OwnedGames
        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = _context.Users.Include(y => y.gcUser.OwnedGames).ThenInclude(x=>x.GameData).FirstOrDefault(x => x.Id.Equals(userId));

            return View(user.gcUser.OwnedGames);
        }

        // GET: OwnedGames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ownedGame = await _context.OwnedGame
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ownedGame == null)
            {
                return NotFound();
            }

            return View(ownedGame);
        }

        // GET: OwnedGames/Create
        public IActionResult Create(int selectedGameId)
        {
            TempData["selectedItem"] = selectedGameId;
            return View();
        }

        // POST: OwnedGames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PurchasePrice,CurrentValue,NewInBox,HasManual,HasBox,Condition,PurchaseDate")] OwnedGame ownedGame, IFormFile upload)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                ApplicationUser user = _context.Users.Include(y => y.gcUser).FirstOrDefault(x => x.Id.Equals(userId));
                ownedGame.GCUserId = user.gcUser.Id;
                ownedGame.GameDataId = int.Parse(TempData["selectedItem"].ToString());
                if(upload!=null&&upload.Length>0)
                {
                    using(Stream fileStream=new FileStream(@"C:\Users\Chiz\source\repos\GCSite\GCSite\wwwroot\3DModel\"+upload.FileName,FileMode.Create))
                    {
                        await upload.CopyToAsync(fileStream);
                    }
                    if(System.IO.File.Exists(@"C:\Users\Chiz\source\repos\GCSite\GCSite\wwwroot\3DModel\" + upload.FileName))
                    {
                        ownedGame.ModelPath = upload.FileName;
                    }
                }
                _context.Add(ownedGame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ownedGame);
        }

        [HttpPost]
        public async Task<IActionResult> FindGameToAdd(string searchString)
        {
            IQueryable<Game> objList;
            if (!String.IsNullOrEmpty(searchString))
                objList = _context.Games.Where(s => s.GameName.Contains(searchString));
            else
                objList = _context.Games;
            TempData["searchString"] = searchString;
            return View(await objList.ToListAsync());
        }

        public async Task<IActionResult> SearchToAdd(string searchString)
        {
            List<GameSearchViewModel> objList = new List<GameSearchViewModel>();
            if (searchString != "" && searchString != null)
            {
                var client = new RestClient("https://api.igdb.com/v4/games");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Client-ID", "4bpvax0sj2c2bdnkfbnw8wup0rzspy");
                request.AddHeader("Authorization", "Bearer " + Environment.GetEnvironmentVariable("IGDBKEY", EnvironmentVariableTarget.User));
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
                    if (!_context.Games.Any(x => x.IGDBId == temp.IGDBId))
                    {
                        await _context.AddAsync(temp);
                        string[] splitString = temp.ThumbnailPath.Split('/');
                        string imgUrl = @"/image/upload/t_thumb/" + splitString[splitString.Length - 1];
                        string localFileName = @"C:\Users\Chiz\source\repos\GCSite\GCSite\wwwroot\image\upload\t_thumb\" + splitString[splitString.Length - 1];
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
                await _context.SaveChangesAsync();
            }

            foreach (GameSearchViewModel gsModel in objList)
            {
                var gamId = await _context.Games.FirstOrDefaultAsync(x => x.IGDBId.Equals(gsModel.IGDBId));
                gsModel.Id = gamId.Id;
            }
            return View(objList);
        }

        // GET: OwnedGames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ownedGame = await _context.OwnedGame.FindAsync(id);
            if (ownedGame == null)
            {
                return NotFound();
            }
            return View(ownedGame);
        }

        // POST: OwnedGames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PurchasePrice,CurrentValue,NewInBox,HasManual,HasBox,Condition")] OwnedGame ownedGame)
        {
            if (id != ownedGame.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ownedGame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnedGameExists(ownedGame.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ownedGame);
        }

        // GET: OwnedGames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ownedGame = await _context.OwnedGame
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ownedGame == null)
            {
                return NotFound();
            }

            return View(ownedGame);
        }

        // POST: OwnedGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ownedGame = await _context.OwnedGame.FindAsync(id);
            _context.OwnedGame.Remove(ownedGame);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OwnedGameExists(int id)
        {
            return _context.OwnedGame.Any(e => e.Id == id);
        }
    }
}
