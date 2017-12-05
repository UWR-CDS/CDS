using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class UserTag
    {
        public int id { get; set; }
        public string sid { get; set; }
        public string name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public int isLogedIn { get; set; }
        public int IsNotViewedCount { get; set; }
        public string ConnectionID { get; set; }

        public string avatar { get; set; }
        public string type { get; set; }

    }
}