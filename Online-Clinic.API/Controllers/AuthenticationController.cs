using Microsoft.AspNetCore.Mvc;
using My_Login_App.API.Auth;
using Online_Clinic.API.Exceptions;
using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;

namespace Online_Clinic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtManager _jwtManager;
        private readonly IPatientRepository _patientRepository;

        public AuthenticationController(IJwtManager jwtManager, IPatientRepository patientRepository)
        {
            _jwtManager = jwtManager;
            _patientRepository = patientRepository;
        }

        //TODO make authentication for every user type

        [HttpPost]
        public IActionResult Authenticate(Login user)
        {
            Patient existingUser = _patientRepository.GetByEmail(user.Email, throwIfNotFound: false);
            if (existingUser == null)
            {
                throw new InvalidCredentialsException("Invalid credentials");
            }
            if(existingUser.Password != user.Password)
            {
                throw new InvalidCredentialsException("Invalid credentials");
            }
            var token = _jwtManager.GetToken(existingUser);
            return Ok(token);
        }
    }
}
