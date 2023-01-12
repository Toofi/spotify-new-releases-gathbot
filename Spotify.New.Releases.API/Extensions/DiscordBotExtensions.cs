using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using RunMode = Discord.Commands.RunMode;

namespace spotify_new_releases.Extensions
{
    public static class DiscordBotExtensions
    {
        public static IServiceCollection AddDiscordSocketClient(this IServiceCollection services)
        {
            var config = new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Debug,
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };
            return services.AddSingleton(config).AddSingleton<DiscordSocketClient>();
        }

        public static IServiceCollection AddDiscordCommandService(this IServiceCollection services)
        {
            var config = new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async
            };
            return services.AddSingleton(config).AddSingleton<CommandService>();
        }
    }
}
