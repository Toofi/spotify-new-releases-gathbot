using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Spotify.New.Releases.Domain.Models.Spotify;
using Spotify.New.Releases.Infrastructure.Repositories;

namespace Spotify.New.Releases.Infrastructure.Extensions
{
    public static class InfrastructureServicesCollectionExtension
    {
        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IAlbumsRepository, AlbumsMongoRepository>()
                .AddScoped<IGenericRepository<Item>, AlbumsMongoRepository>();
        }

        public static IServiceCollection AddMongoConnection(this IServiceCollection services, string connectionString)
        {
            var client = new MongoClient(connectionString);
            try
            {
                client.StartSession();
                var database = client.GetDatabase("releases");
                Console.WriteLine("connection to MongoDb well-established");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Connection failed: {exception.Message}");
                throw;
            }
            return services.AddSingleton<IMongoClient>(new MongoClient(connectionString));
        }
    }
}
