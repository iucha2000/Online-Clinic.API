using My_Login_App.API.Exceptions;
using System.Net;

namespace Online_Clinic.API.Exceptions
{
    public class InvalidCredentialsException : CustomException
    {
        public InvalidCredentialsException(string message) : base(message,StatusCodes.Status404NotFound)
        {

        }
    }
}
