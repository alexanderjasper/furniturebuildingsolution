using Microsoft.Extensions.Configuration;

namespace FurnitureBuildingSolution.Helpers
{
    public class AppSettings
    {
        public string RootUrl { get; set; }
        public string Secret { get; set; }
        public SkyCivSettings SkyCivSettings { get; set; }
        public EmailSettings EmailSettings { get; set; }
        public DatabaseSettings DatabaseSettings { get; set; }

        public static IConfigurationRoot GetConfiguration(string dir, string environmentName)
        {
            if (string.IsNullOrEmpty(environmentName))
                environmentName = "Development";

            var builder = new ConfigurationBuilder()
                .SetBasePath(dir)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        public static AppSettings GetSettings(string dir)
        {
            return GetSettings(dir, null);
        }

        public static AppSettings GetSettings(string dir, string environmentName)
        {
            var config = GetConfiguration(dir, environmentName);
            return GetSettings(config);
        }

        public static AppSettings GetSettings(IConfiguration config)
        {
            return config.Get<AppSettings>();
        }
    }
}
