namespace My_Login_App.API.Exceptions
{
    public class CustomException : Exception
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }

        public CustomException() { }

        public CustomException(string message) : base(message)
        {
            ErrorMessage = message;
        }

        public CustomException(string message, int statusCode) : base(message)
        {
            ErrorMessage = message;
            StatusCode = statusCode;
        }

        public CustomException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorMessage = message;
        }

        public CustomException(string message, int statusCode, Exception innerException) : base(message, innerException)
        {
            ErrorMessage = message;
            StatusCode = statusCode;
        }

        public override string ToString()
        {
            return $"Error: {ErrorMessage}, Status Code: {StatusCode}";
        }
    }
}
