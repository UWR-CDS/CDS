using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class mdl_FeedBack
    {
        public string FeedBackID { get; set; }
        public string LessonId { get; set; }
        public string FeedBackFrom { get; set; }
        public int ID { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public string SchoolName { get; set; }
        public string UserName { get; set; }
        public string Remarks { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public int StatusID { get; set; }
        public int FeedbackStatusID { get; set; }
        public string Type { get; set; }
        public string Frequency { get; set; }
        public string Department { get; set; }
        public int DepartmentID { get; set; }
        public string PreviousDepartment { get; set; }
        public string image { get; set; }
        public int ScopeIdentity { get; set; }
        public List<string> imageList { get; set; }
        public int UserID { get; set; }
        public List<mdl_FeedBackType> feedBackType { get; set; }
        public List<mdl_FeedBackFrequency> feedBackFrequency { get; set; }
        public bool isSchoolFeedBack { get; set; }
        public List<mdl_FeedBackComment> FeedBackComment { get; set; }
    }

    public class mdl_FeedBackType
    {
        public int FeedBackTypeID { get; set; }
        public string TypeName { get; set; }
        public bool IsSelected { get; set; }
    }

    public class mdl_FeedBackFrequency
    {
        public int FeedBackFrequencyID { get; set; }
        public string FrequencyName { get; set; }
    }

    public class mdl_FeedBackComment
    {
        public int FeedBackID { get; set; }
        public int CommentID { get; set; }
        public string CommentDetail { get; set; }
        public DateTime CommentDate { get; set; }
        public string CommentTime { get; set; }
        public int CommentUserID { get; set; }
        public string CommentUserName { get; set; }
    }

    public class mdl_FeedBackImages {
        public int imageID { get; set; }
        public int FileID { get; set; }

        public int BookIDFK { get; set; }
        public int ExtensionIDFK { get; set; }
        public int ChapterIDFK { get; set; }
        public int LessonIDFK { get; set; }
        public string ImageExt { get; set; }
        public string ImageName { get; set; }
        public int MediaGroupType { get; set; }
        public string NickName { get; set; }
        public string ImageSrc { get; set; }

        public string ImageFolderPath { get; set; }
    }
    public class mdl_FeedBackDepartment
    {
        public int FeedBackDepartmentID { get; set; }
        public string DepartmentName { get; set; }
    }

    public class mdl_FeedBackStatus
    {
        public int FeedBackStatusID { get; set; }
        public string StatusName { get; set; }
    }
}