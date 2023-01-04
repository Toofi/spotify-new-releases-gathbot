using Discord;
using Spotify.New.Releases.Domain.Models.Spotify;

namespace Spotify.New.Releases.Application.Services.SpotifyReleasesService
{
    public interface ISpotifyReleasesService
    {
        public Task<EmbedBuilder> GetLatestRelease();
        public Task<List<EmbedBuilder>> GetLatestReleases(uint releasesNumber);
        public Task<List<Item>> GetAllReleases(uint limit = 50);
        public Task Add(Item release);
        public EmbedBuilder CreateEmbeddedRelease(Item release);
    }
}
