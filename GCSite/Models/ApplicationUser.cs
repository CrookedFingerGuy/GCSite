using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GCSite.Models;

namespace GCSite.Models
{
    public class ApplicationUser : IdentityUser
    {
        public GCUser gcUser { get; set; }
    }
}
