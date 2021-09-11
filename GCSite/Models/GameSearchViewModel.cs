using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GCSite.Models
{
    public class GameSearchViewModel
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Game Name")]
        public int IGDBId { get; set; }
        public string GameName { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public string Platform { get; set; }
        public string Genre { get; set; }
        [DisplayName("Release Date")]
        public DateTime? ReleaseDateUs { get; set; }
        [DisplayName("Cover")]
        public string ThumbnailPath { get; set; }
    }
}
