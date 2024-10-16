using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Clinic.API.DTOs;
using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Services;

namespace Online_Clinic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public PasswordController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("Send-Email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _emailService.SendConfirmationCodeAsync(emailRequest.Email);

            return Ok(new { Message = "Email sent successfully" });
        }

        [HttpPost("Verify-Code")]
        public IActionResult VerifyCode(string email, int code)
        {
            var result = _emailService.VerifyCode(email, code);

            return Ok(result);
        }
    }
}
