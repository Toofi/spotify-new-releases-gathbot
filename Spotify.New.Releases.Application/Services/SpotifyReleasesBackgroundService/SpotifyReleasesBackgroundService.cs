using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spotify.New.Releases.Application.Services.SpotifyConnectionService;
using Spotify.New.Releases.Domain.Models.Spotify;

namespace Spotify.New.Releases.Application.Services.SpotifyReleasesBackgroundService
{
    public class SpotifyReleasesBackgroundService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<SpotifyReleasesBackgroundService> _logger;
        private Timer? _timer = null;
        private ISpotifyConnectionService _spotifyConnectionService;

        public SpotifyReleasesBackgroundService(ILogger<SpotifyReleasesBackgroundService> logger, ISpotifyConnectionService spotifyConnectionService)
        {
            _logger = logger;
            _spotifyConnectionService = spotifyConnectionService;
        }
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{datetime} - {service} running.", DateTimeOffset.Now, nameof(SpotifyReleasesBackgroundService));
            _timer = new Timer(Heartbeat, null, TimeSpan.Zero,
                TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        private void Heartbeat(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);
            _logger.LogInformation("{datetime} - {service} is working. Count: {count}", 
                DateTimeOffset.Now, 
                nameof(SpotifyReleasesBackgroundService), 
                count);

            _ = GetLatestReleases();
        }

        private async Task GetLatestReleases()
        {
            try
            {
                List<Item> rawReleases = await this._spotifyConnectionService.GetAllRawReleases();
                _logger.LogInformation("{datetime} - {service} - Successfully received raw data from Spotify. Number of releases received: {count}",
                    DateTimeOffset.Now,
                    nameof(SpotifyReleasesBackgroundService),
                    rawReleases.Count);
            }
            catch (Exception error)
            {
                _logger.LogError("{datetime} - {service} - An error occurred while getting raw data from Spotify : {error}",
                    DateTimeOffset.Now,
                    nameof(SpotifyReleasesBackgroundService),
                    error);
                throw;
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SpotifyReleasesBackgroundService is stopping.");
            _logger.LogInformation("{datetime} - {service} is stopping",
                DateTimeOffset.Now,
                nameof(SpotifyReleasesBackgroundService));
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
