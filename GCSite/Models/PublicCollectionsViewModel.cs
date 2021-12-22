using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCSite.Models
{
    public class PublicCollectionsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime LastLogin { get; set; }
        public TimeSpan MemberSince { get; set; }
        public string LastIP { get; set; }
        public int NumberOfGames { get; set; }
    }
}
