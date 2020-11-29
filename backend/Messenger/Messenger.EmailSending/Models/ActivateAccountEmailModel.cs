using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.EmailSending.Models
{
    public class ActivateAccountEmailModel
    {
        public string userName { get; set; }
        public string link { get; set; }
        public string userEmail { get; set; }
    }
}
