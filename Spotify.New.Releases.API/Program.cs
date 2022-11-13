using Spotify.New.Releases.API.Controllers;
using Spotify.New.Releases.Application.Services.SpotifyConnectionService;
using Spotify.New.Releases.API.Commands;

public class Program
{
    public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

    public async Task MainAsync()
    {
        SpotifyConnectionService spotifyConnectionService = new SpotifyConnectionService();
        SpotifyController controller = new SpotifyController(spotifyConnectionService);

        await DiscordCommandHandler.InstallDiscordBot();
        await Task.Delay(-1);   
    }
}