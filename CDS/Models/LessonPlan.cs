using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class LessonInfo
    {
        public string LessonTitle { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public int Duration { get; set; }
        public int BookID { get; set; }
        public string BookName { get; set; }
        public string EncBookID { get; set; }
        public int SubjectID { get; set; }
        public int ClassID { get; set; }
        public int LanguageID { get; set; }
        public int LessonID { get; set; }
        public string EncryptLessonID { get; set; }
        public string EncryptBookID { get; set; }
        public int ChapterID { get; set; }
        public int LessonPlanStatus { get; set; }
        public int SelectedPageID { get; set; }
        public int SelectedPage { get; set; }
        public int LessonNumber { get; set; }
        public int ChapterNumber { get; set; }
        public int UnitNumber { get; set; }
        public int LogId { get; set; }
        public int SelectedLesspnPlanSection { get; set; }
        public string LessonCode { get; set; }
        public string UnitName { get; set; }
        public string ViewMode { get; set; }
        public string LessonPlanID { get; set; }
        public string HTMLDATA { get; set; }
        public string EncLessonOwnerID { get; set; }
        public List<Section> sections { get; set; }
        public List<SubSection> subsections { get; set; }
        public List<BLL_ChatHistory> lstComments { get; set; }
        public List<LessonImages> ListImages { get; set; }
        public List<LessonImages> ListSounds { get; set; }
        public List<LessonImages> ListVideos { get; set; }
        public List<BookViewData> TotalPagesofBook { get; set; }
        public List<BookLessoninfo> TotalLessonofBook { get; set; }
        public List<BookType> _booksTypes { get; set; }
        public List<LessonTemplate> lstTemplates { get; set; }
        public LessonUser lstLessonUsers { get; set; }
        public List<LessonUser> LessonUserAccess { get; set; }
        public LessonSourceInformation LessSourinfo { get; set; }
        public bool isScriptAudio { get; set; }
        public string ScriptAudioPath { get; set; }
        public string LessonPlanIDDecr { get; set; }
        public string selectedbookoption { get; set; }
        public int BookTypeID { get; set; }
        public string EncBookTypeID { get; set; }
        public List<string> TotalPagesIamges { get; set; }
        public int LessonType { get; set; }
        public int AutoSaveTime { get; set; }
        public int CurriculumID { get; set; }
        public string CuriculumName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string CompleteDate { get; set; }
        public DateTime CompleteDateTime { get; set; }
        public string VerificationDate { get; set; }
        public string VerifiedBy { get; set; }
        public string TimeConsumed { get; set; }
        public string TimeConsumedAgo { get; set; }
        public int Rating { get; set; }
        public int OwnerId { get; set; }
        public string CreatedBy { get; set; }
        public int VerifiedID { get; set; }
        public string LockedBy { get; set; }
        public int RejectCount { get; set; }
        public int VerifyCount { get; set; }
        public int IsLockByVerifier { get; set; }
        public int VerifiedLockTrackId { get; set; }
        public string AllScript { get; set; }
        public string VocabloryCount { get; set; }
    }
    public class LessonImages
    {
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
        public string EncName { get; set; }
        public int UserID { get; set; }
        public string ImageSrc { get; set; }
        public string ImageFolderPath { get; set; }
    }
    public class SaveData
    {
        public string SData { get; set; }
        public int Trackingid { get; set; }
    }
    public class LessonPlanEditorUserWork
    {
        public int BookTypeID { get; set; }
        public int IsIndexPage { get; set; }
        public int ID { get; set; }
        public int LessonPlanID { get; set; }
        public string SectionName { get; set; }
        public string Direction { get; set; }
        public List<SectionHistory> _lstSectionhistory { get; set; }

    }
    public class SectionHistory
    {
        public int LogID { get; set; }
        public int LessonPlanSectionID { get; set; }
        public string LessonPlanScript { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
    public class LessonVerifiedHistory {
        public string UserName { get; set; }
        public string Date { get; set; }
        public string CompletionDate { get; set; }
        public string Reason { get; set; }
    }
    public class ListLessonInfo {
        public List<LessonInfo> lstLessonInfo { get; set; }
        public List<LessonVerifiedHistory> VerificationHistory { get; set; }
        public List<LessonVerifiedHistory> RejectionHistory { get; set; }
    }
    public class LessonSourceInformation
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Rating { get; set; }
        public string LessonVotes { get; set; }
        public string ToatlLessons { get; set; }
        public string ToatlVotes { get; set; }
    }
    public class LessonLock
    {
        public string LockedBy { get; set; }
        public int IsLockByVerifier { get; set; }
        public int VerifiedLockTrackId { get; set; }
    }
}