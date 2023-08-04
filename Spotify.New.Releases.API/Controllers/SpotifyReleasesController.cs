using Microsoft.AspNetCore.Mvc;
using Spotify.New.Releases.Application.Services.SpotifyReleasesService;
using Spotify.New.Releases.Domain.Models.Spotify;

namespace Spotify.New.Releases.API.Controllers
{
    [ApiController]
    [Route("api/getlatest")]
    [Produces("application/json")]
    public class SpotifyReleasesController : ControllerBase
    {
        private ISpotifyReleasesService _spotifyReleasesService { get; set; }
        public SpotifyReleasesController(ISpotifyReleasesService spotifyReleasesService)
        {
            this._spotifyReleasesService = spotifyReleasesService;
        }

        /// <summary>
        /// Get the latest release from Spotify itself.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<string>> GetLatestRelease()
        {
            Item result = await this._spotifyReleasesService.GetLatestRelease();
            return this.Ok(result);
        }

        /// <summary>
        /// Get the latest releases from Spotify itself.
        /// </summary>
        /// <param name="limitNumber"></param>
        /// <returns></returns>
        [HttpGet("{limitNumber}")]
        public async Task<ActionResult<string>> GetLatestReleases(string limitNumber)
        {
            bool isParsed = uint.TryParse(limitNumber, out uint parsedNumber);
            if(!isParsed)
            {
                return this.BadRequest();
            }
            List<Item> results = await this._spotifyReleasesService.GetNumberedLatestReleases(parsedNumber != 0 ? parsedNumber : 50);
            return this.Ok(results);
        }
    }
}
