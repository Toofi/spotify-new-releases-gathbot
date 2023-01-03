using Discord;

namespace Spotify.New.Releases.Application.Services.DiscordMessagesService
{
    public interface IDiscordMessagesService
    {
        public Task SendMessageToGuildAsync(ulong guildId, string message);
        public Task SendEmbeddedMessageToAllGuildsAsync(EmbedBuilder embeddedMessage);
    }
}
