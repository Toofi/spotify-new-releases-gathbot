using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using spotify_new_releases;

public class Program
{
    public static void Main(string[] args) => new Program().MainAsync(args).GetAwaiter().GetResult();

    public async Task MainAsync(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        IServiceCollection services = new ServiceCollection().ConfigureServices();
        Action<IServiceCollection> servicesDelegate = services => services.ConfigureServices();
        builder.ConfigureServices(servicesDelegate);
        builder.Build().Run();
        await Task.Delay(-1);   
    }
}