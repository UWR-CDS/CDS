using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class TemplateModel
    {
        public List<LessonTemplate> lst { get; set; }
        public TemplateDetail obj { get; set; }
        public int SeletedTemplate { get; set; }
        public int SeletedLessonId { get; set; }
        public int SeletedBookID { get; set; }
        public int IsExist { get; set; }
        public int LanguageID { get; set; }
    }
    public class LessonTemplate
    {
        public int TemplateID { get; set; }
        public int Rating { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int TotalLessons { get; set; }
        public int VerifiedLessons { get; set; }
        public int TotalVotes { get; set; }
        public int TempLessonPlanID { get; set; }
    }
    public class TemplateDetail
    {
        public List<Section> lstSection { get; set; }
        public List<SubSection> lstSubsection { get; set; }
    }
    public class TemplateRating
    {
        public int Rating { get; set; }
        public int TotalVotes { get; set; }
        public int FiveVotes { get; set; }
        public int FourVotes { get; set; }
        public int ThreeVotes { get; set; }
        public int TwoVotes { get; set; }
        public int OneVotes { get; set; }
        public int RatedBy { get; set; }
        public string PersonName { get; set; }
        public int PersonRating { get; set; }
        public string Date { get; set; }
        public int CanRate { get; set; }
    }
}