using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spotify.New.Releases.Application.Extensions;
using Spotify.New.Releases.Infrastructure.Extensions;
using spotify_new_releases.Extensions;

public class Program
{
    public static void Main(string[] args) => new Program().MainAsync(args).GetAwaiter().GetResult();

    public async Task MainAsync(string[] args)
    {
        Action<IServiceCollection> servicesDelegate = services => 
            services.AddRedisConnection()
                    .AddInfrastructureRepositories()
                    .AddApplicationServices()
                    .AddDiscordBot().Result
                    .AddCustomOpenApi();
        IHostBuilder builder = Host.CreateDefaultBuilder(args).ConfigureServices(servicesDelegate);
        builder.Build().Run();
        await Task.Delay(-1);
    }
}