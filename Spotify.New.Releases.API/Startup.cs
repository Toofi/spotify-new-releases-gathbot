using Microsoft.Extensions.DependencyInjection;
using Spotify.New.Releases.Application.Extensions;
using Spotify.New.Releases.Infrastructure.Extensions;
using spotify_new_releases.Extensions;

namespace spotify_new_releases
{
    public static class Startup
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            return services
            .AddRedisConnection()
            .AddInfrastructureRepositories()
            .AddApplicationServices()
            .AddDiscordBot().Result;
        }
    }
}
