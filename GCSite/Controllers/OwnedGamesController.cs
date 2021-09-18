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

namespace GCSite.Controllers
{
    public class OwnedGamesController : Controller
    {
        private readonly ApplicationDbContext _context;        

        public OwnedGamesController(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create([Bind("Id,PurchasePrice,CurrentValue,NewInBox,HasManual,HasBox,Condition")] OwnedGame ownedGame)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                ApplicationUser user = _context.Users.Include(y => y.gcUser).FirstOrDefault(x => x.Id.Equals(userId));
                ownedGame.GCUserId = user.gcUser.Id;
                ownedGame.GameDataId = int.Parse(TempData["selectedItem"].ToString());
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

            return View(await objList.ToListAsync());
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
