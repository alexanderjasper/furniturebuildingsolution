namespace FurnitureBuildingSolution.Helpers
{
    public class EmailSettings
    {
        public string SmtpAddress { get; set; }
        public int SmtpPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string CertificateThumbprint { get; set; }
        public int MaxSendAttempts { get; set; }
        public string ApiKey { get; set; }
    }
}
