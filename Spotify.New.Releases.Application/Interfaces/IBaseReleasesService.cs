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
        /// Get the latest releases found, last 50 by default if not specified in parameter.
        /// </summary>
        /// <param name="releasesNumber"></param>
        /// <returns></returns>
        public Task<List<Item>> GetLatestReleases(uint releasesNumber = 50);
    }
}
