using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureBuildingSolution.Database
{
    [Table("Emails")]
    public class DbEmail : BaseDbEntity
    {
        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
        public string Subject { get; set; }
        public string Receiver { get; set; }
        public DateTime? Sent { get; set; }
        public int FailedSendAttempts { get; set; }
    }
}