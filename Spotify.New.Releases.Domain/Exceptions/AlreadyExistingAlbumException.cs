using Spotify.New.Releases.Domain.Enums;

namespace Spotify.New.Releases.Domain.Exceptions
{
    public class AlreadyExistingAlbumException : SnrBaseException
    {
        public AlreadyExistingAlbumException(string exceptionLocation, string albumId) : base(ExceptionType.AlreadyExistingAlbum, exceptionLocation, $"Already existing album. Id: {albumId}")
        {

        }
    }
}
