using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCSite
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Cover
    {
        public int id { get; set; }
        public bool alpha_channel { get; set; }
        public bool animated { get; set; }
        public int game { get; set; }
        public int height { get; set; }
        public string image_id { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public string checksum { get; set; }
    }

    public class Genre
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Company
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class InvolvedCompany
    {
        public int id { get; set; }
        public Company company { get; set; }
        public int created_at { get; set; }
        public bool developer { get; set; }
        public int game { get; set; }
        public bool porting { get; set; }
        public bool publisher { get; set; }
        public bool supporting { get; set; }
        public int updated_at { get; set; }
        public string checksum { get; set; }
    }

    public class Platform
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class GameRoot
    {
        public int id { get; set; }
        public Cover cover { get; set; }
        public int first_release_date { get; set; }
        public List<Genre> genres { get; set; }
        public List<InvolvedCompany> involved_companies { get; set; }
        public string name { get; set; }
        public List<Platform> platforms { get; set; }
    }
}
