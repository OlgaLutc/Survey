using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Encryption.Models
{
    public class EncryptionModel
    {
        public string Word { get; set; }
        public string EncryptedText { get; set; }
        public string DecryptedText {  get; set; }
        public string ActionType { get; set; }
    }
}