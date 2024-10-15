using My_Login_App.API.Exceptions;

namespace Online_Clinic.API.Exceptions
{
    public class FileUploadException : CustomException
    {
        public FileUploadException(string message) : base(message, StatusCodes.Status400BadRequest)
        {
            
        }
    }
}
