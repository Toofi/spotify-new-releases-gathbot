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

        public Task<Item> GetLatestRelease()
        {
            throw new NotImplementedException();
        }

        public Task<List<Item>> GetLatestReleases(uint releasesNumber = 50)
        {
            throw new NotImplementedException();
        }
    }
}
