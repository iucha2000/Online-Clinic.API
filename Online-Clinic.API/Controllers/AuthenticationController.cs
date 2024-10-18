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
        private readonly IAccountRepository _accountRepository;

        public AuthenticationController(IJwtManager jwtManager, IAccountRepository accountRepository)
        {
            _jwtManager = jwtManager;
            _accountRepository = accountRepository;
        }

        //TODO think about storing info in token - user or doctor for easier fetch

        [HttpPost("Login")]
        public IActionResult Authenticate(Login user)
        {
            Account existingUser = _accountRepository.ValidateUser(user.Email, user.Password);
            var token = _jwtManager.GetToken(existingUser);

            return Ok(token);
        }
    }
}
