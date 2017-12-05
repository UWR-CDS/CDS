using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class QuestionAnswersModel
    {
        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
        public int QuestionType { get; set; }
        public int QuestionTypeID { get; set; }
        public string PosibleAnswers { get; set; }
        public string CorrectAnswer { get; set; }
        public string[] lstmcq { get; set; }
        public string[] lstimage { get; set; }
        public string[] lstimagepath { get; set; }
        public bool ispathexit { get; set; }
        public int AnswerTypeIDFK { get; set; }
    }
}