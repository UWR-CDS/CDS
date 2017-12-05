using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class Les_Subject
    {
        public int ClassID { get; set; }
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public int SubjectOrder { get; set; }
        public bool SubjectStatus { get; set; }
        public int LanguageIDFK { get; set; }
        public int IsDLTask { get; set; }

        public string ClassName { get; set; }
        public string BookName { get; set; }
        public int NoofLesson { get; set; }
        public int NoofChapters { get; set; }
    }
}