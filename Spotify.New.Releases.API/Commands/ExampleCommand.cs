using Discord;
using Discord.Commands;

namespace Spotify.New.Releases.API.Commands
{
    public class ExampleCommand : ModuleBase<SocketCommandContext>
    {
        [Command("hello")]
        public async Task Hello()
        {
            await Context.Message.ReplyAsync($"Hello {Context.User.Username}. Nice to meet you!");
        }
    }
}
