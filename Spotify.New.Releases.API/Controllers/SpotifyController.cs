using Spotify.New.Releases.Application.Services.SpotifyConnectionService;

namespace Spotify.New.Releases.API.Controllers
{
    public class SpotifyController
    {
        private ISpotifyConnectionService _spotifyConnectionService { get; set; }
        public SpotifyController(ISpotifyConnectionService spotifyConnectionService)
        {
            this._spotifyConnectionService = spotifyConnectionService;
        }

        public async Task SayHi()
        {
            await this._spotifyConnectionService.Connection();
        }
    }
}
