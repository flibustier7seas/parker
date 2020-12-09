using GitLab.Parker.Abstractions;
using GitLab.Parker.BotCommands;
using GitLab.Parker.Configuration;
using GitLab.Parker.Logic;
using GitLabApiClient;
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

            services.AddMvc();

            services.AddOptions();

            services.AddHttpContextAccessor();

            services.AddSingleton<IBotCommand, StartCommand>();

            services.AddSingleton<IGitLabClient>(x => new GitLabClient(
                "https://git.skbkontur.ru/",
                credentials["GitLabToken"]));

            services.AddScoped<IBotUpdateHandler, BotUpdateHandler>();
            services.AddSingleton<IBotService, BotService>();

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
