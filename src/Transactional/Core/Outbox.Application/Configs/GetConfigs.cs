using Microsoft.Extensions.Configuration;

namespace Outbox.Application.Configs
{
    public static class GetConfig
    {
        private static IConfiguration GetConfiguration()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static string GetDatabaseConfig()
        {
            IConfiguration _configuration = GetConfiguration();

            return _configuration["DatabaseUrl"];
        }
    }
}