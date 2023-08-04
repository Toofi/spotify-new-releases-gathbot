using Spotify.New.Releases.Domain.Models.Spotify;

namespace Spotify.New.Releases.Application.Interfaces
{
    public interface IBaseReleasesService
    {
        /// <summary>
        /// Get the latest release found.
        /// </summary>
        /// <returns></returns>
        public Task<Item> GetLatestRelease();
        /// <summary>
        /// Get all the latest releases found.
        /// </summary>
        /// <param name="releasesNumber"></param>
        /// <returns></returns>
        public Task<List<Item>> GetLatestReleases(uint releasesNumber);
    }
}
