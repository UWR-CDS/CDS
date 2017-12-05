using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class Privilige
    {
        public string PriviligeId { get; set; }

        public string PriviligeName { get; set; }

        public int IsActive { get; set; }

        public string EntityIdFk { get; set; }

        public int pvlId { get; set; }

        public bool IsAllow { get; set; }
        public string EntityName { get; set; }
    }
}