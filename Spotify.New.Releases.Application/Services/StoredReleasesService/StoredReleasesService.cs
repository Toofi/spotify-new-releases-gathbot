using Spotify.New.Releases.Domain.Models.Spotify;
using Spotify.New.Releases.Infrastructure.Repositories;

namespace Spotify.New.Releases.Application.Services.StoredReleasesService
{
    public class StoredReleasesService : IStoredReleasesService
    {
        private readonly IAlbumsRepository _albumsRepository;

        public StoredReleasesService(IAlbumsRepository albumsRepository)
        {
            this._albumsRepository = albumsRepository;
        }

        public async Task<Item> GetLatestRelease()
        {
            return await this._albumsRepository.GetLatestRelease();
        }

        public Task<List<Item>> GetLatestReleases(uint releasesNumber)
        {
            throw new NotImplementedException();
        }
    }
}
