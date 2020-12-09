using GitLab.Parker.Abstractions;
using GitLab.Parker.BotCommands;
using GitLab.Parker.Configuration;
using GitLab.Parker.Logic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GitLab.Parker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var credentials = Configuration.GetSection("Credentials");
            services.Configure<Credentials>(credentials);
            services.Configure<GitLabOptions>(Configuration.GetSection("GitLab"));
            services.Configure<EnvironmentsOptions>(Configuration.GetSection("Environments"));
            services.Configure<EnvironmentsData>(Configuration);

            services.AddMvc();

            services.AddOptions();

            services.AddHttpContextAccessor();

            services.AddSingleton<IBotCommand, StartCommand>();
            services.AddSingleton<IBotCommand, TakeCommand>();
            services.AddSingleton<IBotCommand, FreeCommand>();
            services.AddSingleton<IBotCommand, ListCommand>();
            services.AddSingleton<IBotCommand, AddCommand>();

            services.AddScoped<IBotUpdateHandler, BotUpdateHandler>();
            services.AddSingleton<IBotService, BotService>();
            services.AddSingleton<IEnvironmentStorage, EnvironmentStorage>();

            services.AddHostedService<PollingService>();

            services
                .AddControllers()
                .AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
