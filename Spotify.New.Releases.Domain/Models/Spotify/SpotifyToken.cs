namespace Spotify.New.Releases.Domain.Models.Spotify
{
    public class SpotifyToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public long expires_in { get; set; }
    }
}
