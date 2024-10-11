using My_Login_App.API.Exceptions;

namespace Online_Clinic.API.Exceptions
{
    public class UserNotFoundException : CustomException   
    {
        public UserNotFoundException(string message) : base(message, StatusCodes.Status404NotFound)
        {
            
        }
    }
}
