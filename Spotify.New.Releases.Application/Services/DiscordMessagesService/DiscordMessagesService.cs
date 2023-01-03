using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Spotify.New.Releases.Application.Services.DiscordMessagesService
{
    public class DiscordMessagesService : IDiscordMessagesService
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly ILogger<DiscordMessagesService> _logger;
        public DiscordMessagesService(DiscordSocketClient discordSocketClient, ILogger<DiscordMessagesService> logger)
        {
            _discordSocketClient = discordSocketClient;
            _logger = logger;
        }

        public async Task SendMessageToGuildAsync(ulong guildId, string message)
        {
            SocketGuild guild = _discordSocketClient.GetGuild(guildId);
            SocketTextChannel channel = this.GetFirstTextChannel(guild);
            await channel.SendMessageAsync(message);
        }

        public async Task SendEmbeddedMessageToAllGuildsAsync(EmbedBuilder embeddedMessage)
        {
            try
            {
                IReadOnlyCollection<SocketGuild> guild = _discordSocketClient.Guilds;
                List<SocketTextChannel> textChannels = this.GetSocketTextChannels(guild);
                foreach(SocketTextChannel textChannel in textChannels)
                {
                    await textChannel.SendMessageAsync(embed: embeddedMessage.Build());
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("{datetime} - {service} - There is an error in sending embedded message",
                    DateTimeOffset.Now,
                    nameof(DiscordMessagesService));
                throw;
            }
        }

        private SocketTextChannel GetFirstTextChannel(SocketGuild guild)
        {
            return guild.TextChannels?.FirstOrDefault(channel => channel.GetChannelType() == ChannelType.Text);
        }

        private List<SocketTextChannel> GetSocketTextChannels(IReadOnlyCollection<SocketGuild> guilds)
        {
            List<SocketTextChannel> channels = new List<SocketTextChannel>();
            foreach(SocketGuild guild in guilds)
            {
                channels.Add(this.GetFirstTextChannel(guild));
            }
            return channels;
        }
    }
}
