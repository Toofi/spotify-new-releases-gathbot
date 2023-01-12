using Discord;
using Discord.Commands;
using Spotify.New.Releases.Application.Services.DiscordMessagesService;

namespace Spotify.New.Releases.API.Commands
{
    public class SpotifyReleasesDiscordCommand : ModuleBase<SocketCommandContext>
    {
        private IDiscordMessagesService _discordMessagesService { get; set; }

        public SpotifyReleasesDiscordCommand(IDiscordMessagesService discordMessagesService)
        {
            this._discordMessagesService = discordMessagesService;

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
            Embed[] embeddedReleases = await this._discordMessagesService.GetLatestEmbeddedReleases(parsedNumber);
            await Context.Message.ReplyAsync(embeds: embeddedReleases);
        }

        private async Task GetLatestRelease()
        {
            await Context.Message.ReplyAsync($"Looking for latest release ...");
            var result = await this._discordMessagesService.GetLastEmbeddedRelease();
            await Context.Message.ReplyAsync(embed: result);
        }
    }
}
