using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using spotify_new_releases;
using System.Net;

public class Program
{
    public static void Main(string[] args) => new Program().MainAsync(args).GetAwaiter().GetResult();

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>()
                .UseConfiguration(new ConfigurationBuilder().Build())
                    .UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Loopback, 5001);
                        options.Listen(IPAddress.Loopback, 5002);
                    })
                .UseDefaultServiceProvider(options => options.ValidateScopes = false);
            });
    }

    public async Task MainAsync(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }
}