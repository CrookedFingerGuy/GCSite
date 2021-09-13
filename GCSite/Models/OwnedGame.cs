using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GCSite.Models
{
    public class OwnedGame
    {
        [Key]
        public int Id { get; set; }
        public Game GameData { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal CurrentValue { get; set; }
        public bool NewInBox { get; set; }
        public bool HasManual { get; set; }
        public bool HasBox { get; set; }
        [MaxLength(64)]
        public string Condition { get; set; }
        public int GCUserId { get; set; }
        public GCUser gcuser { get; set; }
    }
}
