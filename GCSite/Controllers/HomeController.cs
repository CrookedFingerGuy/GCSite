using GCSite.Models;
using GCSite.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace GCSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction("UserLanding", "Home");
        }

        public IActionResult UserLanding()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = _db.Users.Include(y => y.gcUser.OwnedGames).FirstOrDefault(x => x.Id.Equals(userId));
            return View(user);
        }
        public IActionResult UserLoginLanding(DateTime date)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = _db.Users.Include(y => y.gcUser.OwnedGames).FirstOrDefault(x => x.Id.Equals(userId));
            if (user.gcUser == null)
            {
                user.gcUser = new GCUser();
                user.gcUser.OwnedGames = new List<OwnedGame>();
            }
            GCUser gcu = user.gcUser;
            _db.Attach(gcu);
            gcu.LastLogin = DateTime.Now;
            IPAddress ip = Request.HttpContext.Connection.RemoteIpAddress;
            if(ip!=null)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    ip = System.Net.Dns.GetHostEntry(ip).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
            }
            gcu.LastIP = ip.ToString();
            _db.Entry(gcu).Property(x => x.LastLogin).IsModified = true;
            _db.Entry(gcu).Property(x => x.LastIP).IsModified = true;
            _db.SaveChanges();
            return View(user);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult PublicCollection()
        {
            List<PublicCollectionsViewModel> pcvm = new List<PublicCollectionsViewModel>();
            PublicCollectionsViewModel tempPcvm;
            foreach(GCUser gcu in _db.GCUsers.Include(y => y.OwnedGames))
            {
                tempPcvm = new PublicCollectionsViewModel();
                tempPcvm.Id = gcu.UserId;
                tempPcvm.LastIP = gcu.LastIP;
                tempPcvm.LastLogin = gcu.LastLogin;
                tempPcvm.MemberSince = new TimeSpan(DateTime.Now.Ticks - gcu.FirstLogin.Ticks);
                if (gcu.FirstName != null)
                    tempPcvm.Name = gcu.FirstName;
                else
                    tempPcvm.Name = "Anonymous";
                tempPcvm.NumberOfGames = gcu.OwnedGames.Count;
                pcvm.Add(tempPcvm);
            }
            return View(pcvm);
        }
        
        public IActionResult PublicCollectionIndex(string id)
        {
            ApplicationUser user = _db.Users.Include(y => y.gcUser.OwnedGames).ThenInclude(x => x.GameData).FirstOrDefault(x => x.Id.Equals(id));
            TempData["localPath"] = Helpers.PathAddBackslash(Path.Combine(_webHostEnvironment.WebRootPath, "image", "upload", "t_thumb"));
            TempData["modelPath"] = Helpers.PathAddBackslash(Path.Combine(_webHostEnvironment.WebRootPath, "3DModel"));
            return View(user.gcUser.OwnedGames);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
