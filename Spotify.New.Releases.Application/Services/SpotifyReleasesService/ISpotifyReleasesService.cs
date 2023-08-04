using Spotify.New.Releases.Application.Interfaces;
using Spotify.New.Releases.Domain.Models.Spotify;

namespace Spotify.New.Releases.Application.Services.SpotifyReleasesService
{
    public interface ISpotifyReleasesService : IBaseReleasesService
    {
        public Task AddRelease(Item release);
        /// <summary>
        /// Get all latest releases from each country, and return only the specified last released in time.
        /// </summary>
        /// <param name="releasesNumber"></param>
        /// <returns></returns>
        public Task<List<Item>> GetNumberedLatestReleases(uint releasesNumber);
    }
}
