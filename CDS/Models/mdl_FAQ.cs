using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class mdl_FAQ
    {
        public int ID { get; set; }
        public string Questions { get; set; }
        public string Answers { get; set; }
        public int IsActive { get; set; }
        public int QuestionType { get; set; }
        public int FAQType { get; set; }
    }
}