using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GCSite.Models;

namespace GCSite.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Game> Games { get; set; }
        public DbSet<GCUser> GCUsers { get; set; }
        public DbSet<GCSite.Models.OwnedGame> OwnedGame { get; set; }

        //public DbSet<GCSite.Models.GameSearchViewModel> GameSearchViewModel { get; set; }
    }
}
