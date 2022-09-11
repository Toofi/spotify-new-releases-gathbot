// See https://aka.ms/new-console-template for more information
using Spotify.New.Releases.API.Controllers;
using Spotify.New.Releases.Application.Services.SpotifyConnectionService;

Console.WriteLine("Hello, World2!");

SpotifyConnectionService spotifyConnectionService = new SpotifyConnectionService();


SpotifyController controller = new SpotifyController(spotifyConnectionService);

await controller.SayHi();