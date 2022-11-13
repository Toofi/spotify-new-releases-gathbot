using Spotify.New.Releases.Application.Services.SpotifyConnectionService;
using System.Web.Mvc;

namespace Spotify.New.Releases.API.Controllers
{
    public class SpotifyController : Controller
    {
        private ISpotifyConnectionService _spotifyConnectionService { get; set; }
        public SpotifyController(ISpotifyConnectionService spotifyConnectionService)
        {
            this._spotifyConnectionService = spotifyConnectionService;
        }

        public async Task GetLatestAlbums()
        {
            await this._spotifyConnectionService.Connection();
        }

        [HttpGet]
        public void GetLatestAlbums(string token)
        {

        }
    }
}
