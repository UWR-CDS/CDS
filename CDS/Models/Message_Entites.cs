using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class Message_Entites
    {
        public int msgid { get; set; }
        public int userid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }

        public string message { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int isactive { get; set; }
    }
}