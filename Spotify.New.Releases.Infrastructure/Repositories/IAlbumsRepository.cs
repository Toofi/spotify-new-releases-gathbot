using Spotify.New.Releases.Domain.Models.Spotify;

namespace Spotify.New.Releases.Infrastructure.Repositories
{
    public interface IAlbumsRepository : IGenericRepository<Item>
    {
        Task<Item> GetLatestRelease();
    }
}
