using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spotify.New.Releases.Application.Extensions;
using Spotify.New.Releases.Application.Handlers;
using Spotify.New.Releases.Infrastructure.Extensions;
using spotify_new_releases.Extensions;

namespace spotify_new_releases
{
    public class Startup
    {
        private IConfiguration _configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddHttpContextAccessor();
            services.AddDiscordSocketClient().AddDiscordCommandService();
            services.AddMongoConnection(this._configuration.GetConnectionString("MongoDb") ?? "")
                    .AddInfrastructureRepositories()
                    .AddApplicationServices()
                    .AddDiscordSocketClient()
                    .AddCustomOpenApi();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {

            var discordSocketClient = services.GetRequiredService<DiscordSocketClient>();
            var discordCommandService = services.GetRequiredService<CommandService>();

            //loggin
            discordSocketClient.LoginAsync(TokenType.Bot, this._configuration.GetSection("DiscordBot").GetValue<string>("token")).GetAwaiter().GetResult();
            discordSocketClient.StartAsync().GetAwaiter().GetResult();

            //config
            discordSocketClient.Log += DiscordBotLoggingHandler.Log;

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spotify New Releases API");
            });
            app.UseCors();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
