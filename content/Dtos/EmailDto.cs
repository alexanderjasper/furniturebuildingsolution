using System;

namespace FurnitureBuildingSolution.Dtos
{
    public class EmailDto : BaseDto
    {
        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
        public string Subject { get; set; }
        public string Receiver { get; set; }
        public DateTime? Sent { get; set; }
        public int FailedSendAttempts { get; set; }
    }
}