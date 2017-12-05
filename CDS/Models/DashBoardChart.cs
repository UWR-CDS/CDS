using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class DashBoardChart
    {
        public int NewLesson { get; set; }
        public int CompleteLesson { get; set; }
        public int VerifyLesson { get; set; }
        public string Date { get; set; }
    }
}