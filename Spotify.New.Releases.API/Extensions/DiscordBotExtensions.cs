using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Spotify.New.Releases.API.Commands;
using System.Reflection;
using RunMode = Discord.Commands.RunMode;

namespace spotify_new_releases.Extensions
{
    public static class DiscordBotExtensions
    {
        public static async Task<IServiceCollection> AddDiscordBot(this IServiceCollection services)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var _discordSocketClient = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Debug,
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            });

            var _discordCommandsService = new CommandService(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async
            });

            _discordSocketClient.Log += DiscordCommandHandler.Log;
            _discordSocketClient.MessageReceived += HandleCommandAsync;
            _discordSocketClient.Ready += HandleNotificationsAsync;
            await _discordCommandsService.AddModulesAsync(Assembly.GetEntryAssembly(), serviceProvider);

            foreach (var module in _discordCommandsService.Modules)
            {
                Console.WriteLine($"{nameof(DiscordCommandHandler)} | Command '{module.Name}' initialized.");
            }
            var discordToken = "";
            await _discordSocketClient.LoginAsync(TokenType.Bot, discordToken);
            await _discordSocketClient.StartAsync();

            async Task HandleNotificationsAsync()
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The discord bot is ready to send notifications.");
                Console.ForegroundColor = ConsoleColor.White;
            }

            async Task HandleCommandAsync(SocketMessage message)
            {
                var socketUserMessage = (SocketUserMessage)message;
                var context = new SocketCommandContext(_discordSocketClient, socketUserMessage);

                if (!DiscordCommandHandler.IsADiscordMessage(socketUserMessage, _discordSocketClient) || message.Author.IsBot)
                {
                    return;
                }
                var result = await _discordCommandsService.ExecuteAsync(context, DiscordCommandHandler.GetArgPos(), serviceProvider);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
            return services
                //.AddSingleton<SpotifyReleasesDiscordCommand>()
                .AddSingleton(_discordSocketClient).AddSingleton(_discordCommandsService);
        }
    }
}
