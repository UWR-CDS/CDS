using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string BookName { get; set; }
    }
    public class BookType
    {
        public string BookTypeId { get; set; }
        public string BookTypeName { get; set; }
    }
    public class BookViewData
    {
        public int PageID { get; set; }
        public int BookPageNum { get; set; }
        public string Src { get; set; }
        public string Title { get; set; }
    }
    public class BookPagesData
    {
        public int PageNo { get; set; }
        public int PageID { get; set; }
        public string PageData { get; set; }
        public int PageLessonNumber { get; set; }
        public int PageLessonID { get; set; }
        public int PageChapterNumber { get; set; }
        public int Status { get; set; }
        public string PageLesson { get; set; }
    }
    public class BookLessoninfo
    {
        public int intLessonID { get; set; }
        public string LessonID { get; set; }
        public string LessonName { get; set; }
        public string ChapterID { get; set; }
        public string ChapterName { get; set; }
        public int Status { get; set; }
        public int ChapterNumber { get; set; }
        public int LessonType { get; set; }
        public int LessonNumber { get; set; }
    }
}