using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spotify.New.Releases.Application.Extensions;
using Spotify.New.Releases.Infrastructure.Extensions;
using spotify_new_releases.Extensions;

namespace spotify_new_releases
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRedisConnection()
                    .AddDiscordBot().Result
                    .AddInfrastructureRepositories()
                    .AddApplicationServices()
                    .AddCustomOpenApi();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
