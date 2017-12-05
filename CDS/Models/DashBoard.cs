using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class DashBoard
    {
        public int TotalLesson { get; set; }
        public int InProgressLesson { get; set; }
        public int VerifiedLesson { get; set; }
        public int CompletedLesson { get; set; }

    }
}