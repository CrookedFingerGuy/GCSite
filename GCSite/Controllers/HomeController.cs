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

namespace GCSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
