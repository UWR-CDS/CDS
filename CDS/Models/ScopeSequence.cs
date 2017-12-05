using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class ScopeSequenceModel
    {
        public List<ScopeSequence> lst { get; set; }
        public List<BookDetail> lstBook { get; set; }
        public int SelectedBookID { get; set; }
    }
    public class ScopeSequence
    {
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public int BookID { get; set; }
        public string BookName { get; set; }
        public int TotalChapters { get; set; }
        public int TotalLessons { get; set; }
        public int UnitNumber { get; set; }
        public int ChapterID { get; set; }
        public int LessonID { get; set; }
        public int IsExist { get; set; }
        public string EncLessonID { get; set; }
        public string LessonTitle { get; set; }
        public int LessonNumber { get; set; }
        public int Duration { get; set; }
        public string Objective { get; set; }
        public string Vocabulary { get; set; }
        public int UnreadCount { get; set; }
        public int HaveTemplate { get; set; }
    }

    public class BookDetail
    {
        public int ChapterID { get; set; }
        public string ChapterName { get; set; }
        public int UnitNumber { get; set; }
        public int LessonID { get; set; }
        public int IsExist { get; set; }
        public string LessonTitle { get; set; }
        public string KeyVocablory { get; set; }
        public string Objectives { get; set; }
        public int BookID { get; set; }
        public string BookName { get; set; }
    }
}