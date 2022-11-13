using Discord;
using Discord.Commands;
using Spotify.New.Releases.Application.Services.SpotifyConnectionService;

namespace Spotify.New.Releases.API.Commands
{
    public class ExampleCommand : ModuleBase<SocketCommandContext>
    {
        private ISpotifyConnectionService _spotifyConnectionService { get; set; }

        public ExampleCommand(ISpotifyConnectionService spotifyConnectionService)
        {
            this._spotifyConnectionService = spotifyConnectionService;
        }

        [Command("latest")]
        public async Task Latest()
        {
            var result = await this._spotifyConnectionService.Connection();
            await Context.Message.ReplyAsync($"Hello {Context.User.Username}. Nice to meet you!");
            await Context.Message.ReplyAsync(embed: result.Build());
        }
    }
}
