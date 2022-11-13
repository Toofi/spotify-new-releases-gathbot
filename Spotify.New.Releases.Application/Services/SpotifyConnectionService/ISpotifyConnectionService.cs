using Discord;

namespace Spotify.New.Releases.Application.Services.SpotifyConnectionService
{
    public interface ISpotifyConnectionService
    {
        public Task<EmbedBuilder> Connection();
    }
}
