using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GCSite.Models
{
    public class GCUser
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [MaxLength(32)]
        public string FirstName { get; set; }
        [MaxLength(32)]
        public string LastName { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime FirstLogin { get; set; }
        [MaxLength(15)]
        public string LastIP { get; set; }
        public List<OwnedGame> OwnedGames { get; set; }
        public bool IsCollectionPublic { get; set; }
    }
}
