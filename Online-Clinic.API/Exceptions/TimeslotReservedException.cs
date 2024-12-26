using My_Login_App.API.Exceptions;

namespace Online_Clinic.API.Exceptions
{
    public class TimeslotReservedException : CustomException
    {
        public TimeslotReservedException(string message) : base(message, StatusCodes.Status409Conflict)
        {
            
        }
    }
}
