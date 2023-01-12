using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Spotify.New.Releases.Application.Extensions;
using Spotify.New.Releases.Application.Handlers;
using Spotify.New.Releases.Infrastructure.Extensions;
using spotify_new_releases.Extensions;

namespace spotify_new_releases
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDiscordSocketClient().AddDiscordCommandService();


            services.AddRedisConnection()
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
            discordSocketClient.LoginAsync(TokenType.Bot, "").GetAwaiter().GetResult();
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
