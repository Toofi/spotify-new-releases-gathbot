using Discord;
using Discord.Commands;
using Spotify.New.Releases.Application.Services.SpotifyReleasesService;

namespace Spotify.New.Releases.API.Commands
{
    public class SpotifyReleasesDiscordCommand : ModuleBase<SocketCommandContext>
    {
        private ISpotifyReleasesService _spotifyConnectionService { get; set; }

        public SpotifyReleasesDiscordCommand(ISpotifyReleasesService spotifyConnectionService)
        {
            this._spotifyConnectionService = spotifyConnectionService;
        }

        [Command("latest")]
        public async Task Latest()
        {
            await this.GetLatestRelease();
        }

        [Command("latest")]
        public async Task Latest(string number)
        {
            await Context.Message.ReplyAsync($"Looking for {number} latest releases ...");
            bool uintParsed = uint.TryParse(number, out uint parsedNumber);
            if (parsedNumber == 0 || parsedNumber == null)
            {
                await Context.Message.ReplyAsync("There is an error. Please check the amount of releases you want to get.");
            }
            if (parsedNumber == 1)
            {
                await this.GetLatestRelease();
            }
            var results = await this._spotifyConnectionService.GetLatestReleases(parsedNumber);
            Embed[] embeddedResults = results.Select(result => result.Build()).ToArray();
            await Context.Message.ReplyAsync(embeds: embeddedResults);
        }

        private async Task GetLatestRelease()
        {
            await Context.Message.ReplyAsync($"Looking for latest release ...");
            var result = await this._spotifyConnectionService.GetLatestRelease();
            await Context.Message.ReplyAsync(embed: result.Build());
        }
    }
}
