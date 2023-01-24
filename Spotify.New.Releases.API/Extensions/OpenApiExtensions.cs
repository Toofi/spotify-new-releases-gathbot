using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace spotify_new_releases.Extensions
{
    public static class  OpenApiExtensions
    {
        public static IServiceCollection AddCustomOpenApi(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo
            { 
                Title = "Spotify New Releases API",
                Version = "v1",
                Description = "An ASP.NET Core Web Api for handling Spotify New Releases data.",
                Contact = new OpenApiContact
                {
                    Name = "Toofi",
                    Url = new Uri("https://linktr.ee/90toofi")
                }
            }));
        }
    }
}
