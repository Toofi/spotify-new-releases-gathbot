using Discord;
using Spotify.New.Releases.Domain.Models.Spotify;

namespace Spotify.New.Releases.Application.Extensions
{
    public static class DiscordEmbeddingExtension
    {
        public static EmbedBuilder CreateEmbeddedRelease(this EmbedBuilder embed, Item release)
        {
            return new EmbedBuilder()
                   .WithAuthor(release.artists.First().name)
                   .WithUrl(release.external_urls.spotify)
                   .WithColor(Color.DarkGreen)
                   .WithDescription($"type: {release.album_type}")
                   .WithTitle(release.name)
                   .WithThumbnailUrl(release.images.First().url)
                   .WithFooter(release.release_date);
        }
    }
}
