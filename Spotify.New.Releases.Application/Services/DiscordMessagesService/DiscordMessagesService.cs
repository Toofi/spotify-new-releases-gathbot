using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spotify.New.Releases.Application.Extensions;
using Spotify.New.Releases.Application.Services.SpotifyReleasesService;
using Spotify.New.Releases.Domain.Models.Spotify;

namespace Spotify.New.Releases.Application.Services.DiscordMessagesService
{
    public class DiscordMessagesService : IDiscordMessagesService
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly IServiceProvider _provider;
        private readonly ILogger<DiscordMessagesService> _logger;
        private readonly ISpotifyReleasesService _spotifyReleasesService;

        public DiscordMessagesService(IServiceProvider provider, ILogger<DiscordMessagesService> logger, ISpotifyReleasesService spotifyReleasesService)
        {
            _provider = provider;
            _discordSocketClient = _provider.GetRequiredService<DiscordSocketClient>();
            _logger = logger;
            _spotifyReleasesService = spotifyReleasesService;
        }

        public async Task SendMessageToGuildAsync(ulong guildId, string message)
        {
            SocketGuild guild = _discordSocketClient.GetGuild(guildId);
            SocketTextChannel channel = this.GetFirstTextChannel(guild);
            await channel.SendMessageAsync(message);
        }

        public async Task SendEmbeddedMessageToAllGuildsAsync(Embed embeddedMessage)
        {
            try
            {
                IReadOnlyCollection<SocketGuild> guild = _discordSocketClient.Guilds;
                List<SocketTextChannel> textChannels = this.GetSocketTextChannels(guild);
                foreach(SocketTextChannel textChannel in textChannels)
                {
                    await textChannel.SendMessageAsync(embed: embeddedMessage);
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

        public async Task<Embed> GetLastEmbeddedRelease()
        {
            Item lastRelease = await this._spotifyReleasesService.GetLatestRelease();
            return new EmbedBuilder().CreateEmbeddedRelease(lastRelease).Build();
        }

        public async Task<Embed[]> GetLatestEmbeddedReleases(uint releasesNumber)
        {
            List<Item> latestReleases = await this._spotifyReleasesService.GetLatestReleases(releasesNumber);
            List<Embed> embeddedReleases = new List<Embed>();
            foreach(Item release in latestReleases)
            {
                embeddedReleases.Add(new EmbedBuilder().CreateEmbeddedRelease(release).Build());
            }
            return embeddedReleases.ToArray();
        }

        /// <summary>
        /// Get the first textChannel in a given guild.
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        private SocketTextChannel GetFirstTextChannel(SocketGuild guild)
        {
            return guild.TextChannels?.FirstOrDefault(channel => channel.GetChannelType() == ChannelType.Text);
        }

        /// <summary>
        /// Get all first textChannels in all subscribed guilds.
        /// </summary>
        /// <param name="guilds"></param>
        /// <returns></returns>
        private List<SocketTextChannel> GetSocketTextChannels(IReadOnlyCollection<SocketGuild> guilds)
        {
            List<SocketTextChannel> channels = new List<SocketTextChannel>();
            foreach (SocketGuild guild in guilds)
            {
                channels.Add(this.GetFirstTextChannel(guild));
            }
            return channels;
        }
    }
}
