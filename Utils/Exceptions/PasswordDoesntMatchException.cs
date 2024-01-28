namespace SmartAlertAPI.Utils.Exceptions
{
    public class PasswordDoesntMatchException: Exception
    {
        public PasswordDoesntMatchException()
        {

        }
        public PasswordDoesntMatchException(string? message) : base(message)
        {

        }

        public PasswordDoesntMatchException(string? message, Exception inner) : base(message, inner)
        {

        }
    }
}
