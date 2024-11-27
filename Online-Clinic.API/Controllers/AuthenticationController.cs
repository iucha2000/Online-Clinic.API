using Microsoft.AspNetCore.Mvc;
using My_Login_App.API.Auth;
using Online_Clinic.API.DTOs;
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
        private readonly IEmailService _emailService;

        public AuthenticationController(IJwtManager jwtManager, IAccountRepository accountRepository, IEmailService emailService)
        {
            _jwtManager = jwtManager;
            _accountRepository = accountRepository;
            _emailService = emailService;
        }

        [HttpPost("Login")]
        public IActionResult Authenticate(Login user)
        {
            Account existingUser = _accountRepository.ValidateUser(user.Email, user.Password);
            var token = _jwtManager.GetToken(existingUser);

            return Ok(token);
        }

        [HttpPost("Send-Confirmation-Code")]
        public async Task<IActionResult> SendConfirmationCodeEmail(string email)
        {
            await _emailService.SendConfirmationCodeAsync(email);

            return Ok(new { Message = "Email sent successfully" });
        }

        [HttpPost("Verify-Confirmation-Code")]
        public IActionResult VerifyConfrmationCode(string email, int code)
        {
            var result = _emailService.VerifyCode(email, code);

            return Ok(result);
        }

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword(string email)
        {
            await _emailService.SendNewPasswordAsync(email);

            return Ok(new { Message = "Password reset email sent successfully" });
        }

        [HttpPost("Change-Password")]
        public IActionResult ChangePassword(ChangePasswordRequest request)
        {
            _accountRepository.UpdateUserPassword(request.Email, request.Password);

            return Ok(new { Message = "Password changed successfully" });
        }
    }
}
