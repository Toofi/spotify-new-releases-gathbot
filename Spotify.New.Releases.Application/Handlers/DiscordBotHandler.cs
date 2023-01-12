using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Spotify.New.Releases.Application.Handlers
{
    public class DiscordBotHandler : IHostedService
    {
        public readonly DiscordSocketClient _client;

        public DiscordBotHandler(DiscordSocketClient client)
        {
            this._client = client;
            this._client.Ready += Client_Ready;
        }

        public async Task Client_Ready()
        {
            var latestCommand = new SlashCommandBuilder()
                .WithName("latest")
                .WithDescription("Get the latest release I received")
                .AddOption("number", ApplicationCommandOptionType.String, "The number of lates releases you would like to get", isRequired: true)
                .Build();
            var helloCommand = new SlashCommandBuilder()
                .WithName("hello")
                .WithDescription("just to say hello")
                .Build();

            List<SlashCommandProperties> commands = new List<SlashCommandProperties>();
            commands.Add(latestCommand);
            commands.Add(helloCommand);

            try
            {
                foreach(SlashCommandProperties command in commands)
                {
                    await _client.CreateGlobalApplicationCommandAsync(command);
                }
            }
            catch (ApplicationCommandException exception)
            {
                string json = JsonConvert.SerializeObject(exception, Formatting.Indented);
                Console.WriteLine(json);
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
