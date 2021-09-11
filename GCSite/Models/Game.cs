using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GCSite.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        public int IGDBId { get; set; }
        [DisplayName("Game Name")]
        public string GameName { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public string Platform { get; set; }
        public string Genre { get; set; }
        public DateTime? ReleaseDateUs { get; set; }
        public DateTime? ReleaseDateJp { get; set; }
        public DateTime? ReleaseDateEu { get; set; }
        public string ThumbnailPath { get; set; }
        public string CoverArtPath { get; set; }
        public string ManualScanPath { get; set; }
        public string ThreeDBoxmodelPath { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal? CurrentValueCib { get; set; }
        public decimal? CurrentValueMo { get; set; }
        public long? GameSizeMb { get; set; }
        public long? GamePlayLength { get; set; }
        public string MinSpecs { get; set; }
        public string BoxStyle { get; set; }
        public string BoxSize { get; set; }
        public string MediaType { get; set; }
        public int? MediaCount { get; set; }
        public string MediaScanPath { get; set; }
    }
}
