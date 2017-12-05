using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class LessonUser
    {
        public int LessonRightsID { get; set; }
        public string LessonRightsName { get; set; }
        public string UserName { get; set; }
        public bool HasRights { get; set; }
        public int LessonOwnerID { get; set; }
        public List<LessonUserRight> LessonUserRightList { get; set; }
        public LessonOwner LessonOwner { get; set; }
    }

    public class LessonOwner
    {
        public string OwnerName { get; set; }
        public int OwnerID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string CompleteDate { get; set; }
        public string TimeConsumed { get; set; }
        public string OwnerEmail { get; set; }
        public string ContactNo { get; set; }
        public int Rating { get; set; }
        public string LastVerifiyBy { get; set; }
    }

    public class LessonUserRight
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Rights { get; set; }
        public string Icon { get; set; }
        public List<string> LessonRightList { get; set; }
        public string UserEmail { get; set; }
        public string UserContactNo { get; set; }
    }
}
