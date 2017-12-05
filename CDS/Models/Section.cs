using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class Section
    {
        public int SerialNo { get; set; }
        public int Id { get; set; }
        public int SubsectionCount { get; set; }
        public int SingleSubsectionId { get; set; }
        public string SectionId { get; set; }
        public string Name { get; set; }
        public string Duration { get; set; }
        public string TabButton { get; set; }
        public int OrderBy { get; set; }
        public int IsActive { get; set; }
        public int LessonPlanId { get; set; }
        public int SectionStatus { get; set; }
        public string LessonId { get; set; }


        public int BookTypeId { get; set; }
        public int BookId { get; set; }
        public int LanguageId { get; set; }

        public string DurationFrom { get; set; }
        public string DurationTo { get; set; }

        public int Action { get; set; }

        public List<string> fromList { get; set; }
        public List<string> toList { get; set; }
    }
    public class SubSection
    {
        public int SectionId { get; set; }
        public int SubSectionId { get; set; }
        public string SubSectionName { get; set; }
        public string SectionName { get; set; }
        public int SubSectionSize { get; set; }
        public string LessonPlanScript { get; set; }
        public int LessonPlanScriptId { get; set; }
    }
}