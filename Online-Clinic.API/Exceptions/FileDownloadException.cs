using My_Login_App.API.Exceptions;

namespace Online_Clinic.API.Exceptions
{
    public class FileDownloadException : CustomException
    {
        public FileDownloadException(string message) : base(message, StatusCodes.Status404NotFound)
        {
            
        }
    }
}
