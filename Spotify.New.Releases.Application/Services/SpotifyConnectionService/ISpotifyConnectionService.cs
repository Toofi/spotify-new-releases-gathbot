using Discord;
using Spotify.New.Releases.Domain.Models.Spotify;

namespace Spotify.New.Releases.Application.Services.SpotifyConnectionService
{
    public interface ISpotifyConnectionService
    {
        public Task<EmbedBuilder> GetLatestRelease();
        public Task<List<EmbedBuilder>> GetLatestReleases(uint releasesNumber);
    }
}
