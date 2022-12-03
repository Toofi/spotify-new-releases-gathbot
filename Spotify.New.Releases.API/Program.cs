using Spotify.New.Releases.API.Controllers;
using Spotify.New.Releases.Application.Services.SpotifyConnectionService;
using Spotify.New.Releases.API.Commands;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public class Program
{
    public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

    public async Task MainAsync()
    {

        var token = "";

        var _discordSocketClient = new DiscordSocketClient(new DiscordSocketConfig()
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
            .AddSingleton(_discordSocketClient)
            .AddSingleton(_commands)
            .AddSingleton<ISpotifyConnectionService, SpotifyConnectionService>()
            .BuildServiceProvider();

        _discordSocketClient.Log += DiscordCommandHandler.Log;
        _discordSocketClient.MessageReceived += HandleCommandAsync;

        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        foreach (var module in _commands.Modules)
        {
            Console.WriteLine($"{nameof(DiscordCommandHandler)} | Command '{module.Name}' initialized.");
        }

        await _discordSocketClient.LoginAsync(TokenType.Bot, token);
        await _discordSocketClient.StartAsync();

        async Task HandleCommandAsync(SocketMessage message)
        {
            var socketUserMessage = (SocketUserMessage)message;
            var context = new SocketCommandContext(_discordSocketClient, socketUserMessage);

            if (!DiscordCommandHandler.IsADiscordMessage(socketUserMessage, _discordSocketClient) || message.Author.IsBot)
            {
                return;
            }
            var result = await _commands.ExecuteAsync(context, DiscordCommandHandler.GetArgPos(), _services);
            if (!result.IsSuccess)
            {
                Console.WriteLine(result.ErrorReason);
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
        await Task.Delay(-1);   
    }
}