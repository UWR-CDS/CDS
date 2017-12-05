using System;
using System.Collections.Generic;
namespace CDS.Models
{
    public class BLL_ChatHistory
    {
        public int Message_ID { get; set; }
        public int UnreadCount { get; set; }
        public int ReceiverUnreadCount { get; set; }
        public int OwnerID { get; set; }
        public string EncMessage_ID { get; set; }
        public string EncLessonID { get; set; }
        public int UserID_A { get; set; }
        public string UserName { get; set; }
        public string EncUserID_A { get; set; }
        public string InvitationID { get; set; }
        public int UserID_B { get; set; }
        public int SenderID { get; set; }
        public string Message { get; set; }
        public DateTime MessageDateTime { get; set; }
        public string CommentTimeAgo { get; set; }
        public string MessageDateTimeStr { get; set; }
        public int TotalPages { get; set; }
        public int IsViewed { get; set; }
        public int LessonID { get; set; }
        public bool isArchive { get; set; }
        public bool isRead { get; set; }
        public int InboxCount { get; set; }

        public string Comment { get; set; }
        public string WhereFrom { get; set; }
        public string CommentTitle { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int textID { get; set; }
        public int sectionID { get; set; }

        public string BookName { get; set; }
        public string SubjectName { get; set; }
        public string LessonName { get; set; }
        public string ClassName { get; set; }

        public Unread unReadObj { get; set; }
    }

    public class Unread
    {
        public List<int> MessageList { get; set; }
        public int UnreadCount { get; set; }
    }
}