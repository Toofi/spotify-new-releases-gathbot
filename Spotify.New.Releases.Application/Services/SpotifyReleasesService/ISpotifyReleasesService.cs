using Spotify.New.Releases.Application.Interfaces;
using Spotify.New.Releases.Domain.Models.Spotify;

namespace Spotify.New.Releases.Application.Services.SpotifyReleasesService
{
    public interface ISpotifyReleasesService : IBaseReleasesService
    {
        public Task AddRelease(Item release);
    }
}
