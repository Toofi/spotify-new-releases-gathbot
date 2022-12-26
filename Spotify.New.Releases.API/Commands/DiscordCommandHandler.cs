using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Spotify.New.Releases.API.Commands
{
    public abstract class DiscordCommandHandler
    {
        public static char _orderPrefix { get; private set; } = '!';
        private static int _argPos = 0;

        public static bool IsThereACommandPrefix(SocketUserMessage message)
        {
            return message.HasCharPrefix(_orderPrefix, ref _argPos);
        }

        public static int GetArgPos()
        {
            return _argPos;
        }

        public static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public static bool IsADiscordMessage(SocketUserMessage socketUserMessage, DiscordSocketClient discordSocketClient)
        {
            return DiscordCommandHandler.IsThereACommandPrefix(socketUserMessage) || socketUserMessage.HasMentionPrefix(discordSocketClient.CurrentUser, ref _argPos);
        }
    }
}
