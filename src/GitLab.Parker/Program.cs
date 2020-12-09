using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GitLab.Parker
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                    builder
                        .AddJsonFile("data.json", true, true)
                        .AddUserSecrets<Startup>())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseShutdownTimeout(TimeSpan.FromMinutes(1));
                    webBuilder.UseStartup<Startup>();
                });
    }
}
