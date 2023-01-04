using Spotify.New.Releases.Application.Services.SpotifyReleasesService;
using System.Web.Mvc;

namespace Spotify.New.Releases.API.Controllers
{
    public class SpotifyController : Controller
    {
        private ISpotifyReleasesService _spotifyConnectionService { get; set; }
        public SpotifyController(ISpotifyReleasesService spotifyConnectionService)
        {
            this._spotifyConnectionService = spotifyConnectionService;
        }

        public async Task GetLatestAlbums()
        {
            await this._spotifyConnectionService.GetLatestRelease();
        }

        [HttpGet]
        public void GetLatestAlbums(string token)
        {

        }
    }
}
