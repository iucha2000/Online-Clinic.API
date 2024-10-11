using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My_Login_App.API.Auth;
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
        //TODO add custom exception model

        [HttpPost]
        public IActionResult Authenticate(Login user)
        {
            Patient existingUser = _patientRepository.GetByEmail(user.Email);
            if (existingUser == null)
            {
                return NotFound(new { message = "Invalid credentials" });
            }
            if(existingUser.Password != user.Password)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
            var token = _jwtManager.GetToken(existingUser);
            return Ok(token);
        }
    }
}
