using Microsoft.Extensions.DependencyInjection;
using Spotify.New.Releases.Application.Services.DiscordMessagesService;
using Spotify.New.Releases.Application.Services.SpotifyConnectionService;
using Spotify.New.Releases.Application.Services.SpotifyReleasesBackgroundService;

namespace Spotify.New.Releases.Application.Extensions
{
    public static class ApplicationServicesCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<ISpotifyConnectionService, SpotifyConnectionService>()
                .AddSingleton<IDiscordMessagesService, DiscordMessagesService>()
                .AddHostedService<SpotifyReleasesBackgroundService>();
        }
    }
}
