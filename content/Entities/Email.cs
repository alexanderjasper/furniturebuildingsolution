using System;
using System.Collections.Generic;

namespace FurnitureBuildingSolution.Entities
{
    public class Email : BaseEntity
    {
        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
        public string Subject { get; set; }
        public string Receiver { get; set; }
        public DateTime? Sent { get; set; }
        public int FailedSendAttempts { get; set; }
    }
}