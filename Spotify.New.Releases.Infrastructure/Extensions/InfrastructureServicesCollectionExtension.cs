using Microsoft.Extensions.DependencyInjection;
using Spotify.New.Releases.Domain.Models.Spotify;
using Spotify.New.Releases.Infrastructure.Repositories;
using StackExchange.Redis;

namespace Spotify.New.Releases.Infrastructure.Extensions
{
    public static class InfrastructureServicesCollectionExtension
    {
        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IGenericRepository<Item>, AlbumsRepository>();
        }

        public static IServiceCollection AddRedisConnection(this IServiceCollection services)
        {
            var redisConnection = ConnectionMultiplexer.Connect(
                new ConfigurationOptions
                {
                    EndPoints = { "redis-16050.c269.eu-west-1-3.ec2.cloud.redislabs.com:16050" },
                    Password = "",
                    User = "default"
                });

            return services.AddSingleton(redisConnection);
        }
    }
}
