namespace FurnitureBuildingSolution.Helpers
{
    public class DatabaseSettings
    {
        public string Host { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool TrustedConnection { get; set; }
        public bool MultipleActiveResultSets { get; set; }
        public string GetConnectionString()
        {
            var connectionString = $"Server={Host};Database={Database};";
            if (UserId != null && Password != null)
            {
                connectionString += $"User Id={UserId};Password={Password};";
            }
            if (TrustedConnection)
            {
                connectionString += "Trusted_Connection=True;";
            }
            if (MultipleActiveResultSets)
            {
                connectionString += "MultipleActiveResultSets=True;";
            }
            return connectionString;
        }
    }
}
