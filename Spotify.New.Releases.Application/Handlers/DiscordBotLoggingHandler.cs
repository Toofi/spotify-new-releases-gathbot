using Discord;

namespace Spotify.New.Releases.Application.Handlers
{
    public abstract class DiscordBotLoggingHandler
    {
        public static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
