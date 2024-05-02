using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Encryption.DB
{
    public class QModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public bool CorrectAnswer { get; set; } 
        public bool? UserAnswer { get; set; } // nullable
        public string Advice {  get; set; }
    }
}