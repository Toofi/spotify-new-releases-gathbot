using Discord.WebSocket;
using Microsoft.Extensions.Hosting;

namespace Spotify.New.Releases.Application.Handlers
{
    public class DiscordBotCommandHandler : IHostedService
    {
        private readonly DiscordSocketClient _client;

        public DiscordBotCommandHandler(DiscordSocketClient client)
        {
            this._client = client;
            this._client.SlashCommandExecuted += SlashCommandHandler;
        }

        public async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "latest":
                    await command.RespondAsync($"You executed {command.Data.Name} and you received will receive the latest release we have stored");
                    break;
                case "hello":
                    await command.RespondAsync("Hello, I am the New Spotify Releases bot. I'm here to gather all the latest releases worldwide in Spotify, compare and store them.");
                    break;
                default:
                    break;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
