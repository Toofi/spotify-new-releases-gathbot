using Spotify.New.Releases.Domain.Enums;

namespace Spotify.New.Releases.Domain.Exceptions
{
    public class SnrBaseException : Exception
    {
        public ExceptionType ExceptionType;
        public string ExceptionLocation;

        public SnrBaseException(string exceptionLocation)
        {
            this.ExceptionLocation = exceptionLocation;
        }

        public SnrBaseException(ExceptionType exceptionType, string exceptionLocation, string message) : base(message)
        {
            this.ExceptionType = exceptionType;
            this.ExceptionLocation = exceptionLocation;
        }
        public SnrBaseException(ExceptionType exceptionType, string message) : base(message)
        {
            this.ExceptionType = exceptionType;
        }

        public SnrBaseException(ExceptionType exceptionType, string exceptionLocation, Exception exception) : base(exception.Message, exception)
        {
            this.ExceptionType = exceptionType;
            this.ExceptionLocation = exceptionLocation;
        }

        public SnrBaseException(string exceptionLocation, Exception exception) : base(exception.Message, exception)
        {
            this.ExceptionLocation = exceptionLocation;
        }
    }
}
