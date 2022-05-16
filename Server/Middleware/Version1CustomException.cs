using System.Globalization;

namespace BlazorWASMDemo.Server.Middleware
{
    public class Version1CustomException : Exception
    {
        public Version1CustomException() : base() { }

        public Version1CustomException(string message) : base(message) { }

        public Version1CustomException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
