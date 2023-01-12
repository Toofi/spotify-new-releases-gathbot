using Microsoft.Extensions.DependencyInjection;
using Spotify.New.Releases.Application.Services.DiscordMessagesService;
using Spotify.New.Releases.Application.Services.SpotifyReleasesService;
using Spotify.New.Releases.Application.Services.SpotifyReleasesBackgroundService;
using Spotify.New.Releases.Application.Handlers;

namespace Spotify.New.Releases.Application.Extensions
{
    public static class ApplicationServicesCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<ISpotifyReleasesService, SpotifyReleasesService>()
                .AddSingleton<IDiscordMessagesService, DiscordMessagesService>()
                .AddHostedService<SpotifyReleasesBackgroundService>()
                .AddHostedService<DiscordBotHandler>()
                .AddHostedService<DiscordBotCommandHandler>();
        }
    }
}
