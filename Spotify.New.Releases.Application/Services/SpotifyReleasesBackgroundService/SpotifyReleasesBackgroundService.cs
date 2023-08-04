using Discord;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spotify.New.Releases.Application.Extensions;
using Spotify.New.Releases.Application.Services.DiscordMessagesService;
using Spotify.New.Releases.Application.Services.SpotifyReleasesService;
using Spotify.New.Releases.Domain.Models.Spotify;
using Spotify.New.Releases.Infrastructure.Repositories;

namespace Spotify.New.Releases.Application.Services.SpotifyReleasesBackgroundService
{
    public class SpotifyReleasesBackgroundService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<SpotifyReleasesBackgroundService> _logger;
        private Timer? _timer = null;
        private readonly ISpotifyReleasesService _spotifyReleasesService;
        private readonly IGenericRepository<Item> _albumsRepository;
        private readonly IDiscordMessagesService _discordMessagesService;

        public SpotifyReleasesBackgroundService(
            ILogger<SpotifyReleasesBackgroundService> logger,
            ISpotifyReleasesService spotifyConnectionService,
            IGenericRepository<Item> albumsRepository,
            IDiscordMessagesService discordMessagesService)
        {
            _logger = logger;
            _spotifyReleasesService = spotifyConnectionService;
            _albumsRepository = albumsRepository;
            _discordMessagesService = discordMessagesService;
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
                List<Item> rawReleases = await this._spotifyReleasesService.GetLatestReleases(50);
                _logger.LogInformation("{datetime} - {service} - Successfully received raw data from Spotify. Number of releases received: {count}",
                    DateTimeOffset.Now,
                    nameof(SpotifyReleasesBackgroundService),
                    rawReleases.Count);
                await this.AddNewReleases(rawReleases);
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

        private async Task AddNewReleases(List<Item> releases)
        {
            foreach (Item release in releases)
            {
                if (!await this.IsReleaseAlreadyExisting(release.id))
                {
                    await this._spotifyReleasesService.AddRelease(release);
                    Embed embeddedRelease = new EmbedBuilder().CreateEmbeddedRelease(release).Build();
                    await this._discordMessagesService.SendEmbeddedMessageToAllGuildsAsync(embeddedRelease);
                }
            }
        }

        private async Task<bool> IsReleaseAlreadyExisting(string releaseId)
        {
            Item release = await this._albumsRepository.GetByIdAsync(releaseId);
            return release != null;
        }
    }
}
