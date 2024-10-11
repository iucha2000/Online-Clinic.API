using My_Login_App.API.Exceptions;

namespace Online_Clinic.API.Exceptions
{
    public class UserAlreadyExistsException : CustomException
    {
        public UserAlreadyExistsException(string message) : base(message, StatusCodes.Status409Conflict)
        {
            
        }
    }
}
