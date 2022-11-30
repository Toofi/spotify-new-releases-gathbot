using Discord;
using Discord.Commands;
using Spotify.New.Releases.Application.Services.SpotifyConnectionService;

namespace Spotify.New.Releases.API.Commands
{
    public class SpotifyReleasesDiscordCommand : ModuleBase<SocketCommandContext>
    {
        private ISpotifyConnectionService _spotifyConnectionService { get; set; }

        public SpotifyReleasesDiscordCommand(ISpotifyConnectionService spotifyConnectionService)
        {
            this._spotifyConnectionService = spotifyConnectionService;
        }

        [Command("latest")]
        public async Task Latest()
        {
            await Context.Message.ReplyAsync($"Looking for latest release ...");
            //faire une policy pour ce process selon le client, ici discord
            var result = await this._spotifyConnectionService.GetLatestRelease();
            await Context.Message.ReplyAsync(embed: result.Build());
        }

        [Command("latest")]
        public async Task Latest(string number)
        {
            await Context.Message.ReplyAsync($"Looking for latest releases ...");
            //faire une policy pour ce process selon le client, ici discord
            bool uintParsed = uint.TryParse(number, out uint parsedNumber);
            var results = await this._spotifyConnectionService.GetLatestReleases(parsedNumber);
            Embed[] embeddedResults = results.Select(result => result.Build()).ToArray();
            await Context.Message.ReplyAsync(embeds: embeddedResults);
        }
    }
}
