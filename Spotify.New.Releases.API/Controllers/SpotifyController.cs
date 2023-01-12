using Microsoft.AspNetCore.Mvc;
using Spotify.New.Releases.Application.Services.SpotifyReleasesService;

namespace Spotify.New.Releases.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SpotifyController : ControllerBase
    {
        private ISpotifyReleasesService _spotifyReleasesService { get; set; }
        public SpotifyController(ISpotifyReleasesService spotifyReleasesService)
        {
            this._spotifyReleasesService = spotifyReleasesService;
        }

        [HttpGet]
        public async Task  GetLatestAlbums()
        {
            this._spotifyReleasesService.GetLatestReleases();
        }
    }
}
