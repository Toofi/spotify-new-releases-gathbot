using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Spotify.New.Releases.API.Commands
{
    public abstract class DiscordCommandHandler
    {
        static char _orderPrefix = '!';
        static int _argPos = 0;

        public static async Task InstallDiscordBot()
        {
            var token = "MTAzNzQwNjkxMDQ3Mjc4NTkzMA.GaEG6x.hL66F2GkDxOtRXpnLHOBm0fY2T8PX8N60sd1yU";

            var _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Debug,
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            });

            var _commands = new CommandService(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async
            });

            IServiceProvider _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            _client.Log += Log;
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            foreach (var module in _commands.Modules)
            {
                Console.WriteLine($"{nameof(DiscordCommandHandler)} | Command '{module.Name}' initialized.");
            }

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            async Task HandleCommandAsync(SocketMessage message)
            {
                var socketUserMessage = (SocketUserMessage)message;
                var context = new SocketCommandContext(_client, socketUserMessage);
                
                if (!(IsThereACommandPrefix(socketUserMessage) || socketUserMessage.HasMentionPrefix(_client.CurrentUser, ref _argPos)) || message.Author.IsBot)
                {
                    return;
                } 
                var result = await _commands.ExecuteAsync(context, _argPos, _services);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }

            Task Log(LogMessage msg)
            {
                Console.WriteLine(msg.ToString());
                return Task.CompletedTask;
            }
        }

        public static bool IsThereACommandPrefix(SocketUserMessage message)
        {
            return message.HasCharPrefix(_orderPrefix, ref _argPos);
        }
    }
}
