using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public enum Priviliges : int
    {
        CanViewMessages = 1,
        CanVerifiyLesson = 2,
        CanRateLesson = 3,
        CanSendComment = 4,
        CanViewWaitingforVerificationLesson = 5,
        CanViewUserList = 6,
        CanViewFeedback = 7,
        CanViewFeedbackManagement = 8,
        CanAddMedia = 10,
        CanDeleteMedia = 11,
        CanRejectLesson = 12,
        CanViewLessonVerifiedList = 13,
        CanViewLessonScript = 14,
        CanViewScopeSequence = 15,
        CanSetFeedBackStatus = 16,
        CanViewVerifiedLessonList = 17

         


    }
}