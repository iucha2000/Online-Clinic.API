using Online_Clinic.API.Interfaces;
using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Online_Clinic.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Online_Clinic.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfirmationRepository _confirmationRepository;
        //private readonly string _sendGridApiKey;

        public EmailService(IEmailConfirmationRepository confirmationRepository)
        {
            _confirmationRepository = confirmationRepository;
            //_sendGridApiKey = "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";
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            //var client = new SendGridClient(_sendGridApiKey);
            //var from = new EmailAddress("test@example.com", "Example User");
            //var subject = "Sending with SendGrid is Fun";
            //var to = new EmailAddress("test@example.com", "Example User");
            //var plainTextContent = "and easy to do anywhere, even with C#";
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //var response = await client.SendEmailAsync(msg);
        }

        public int GenerateConfirmationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }

        public async Task SendConfirmationCodeAsync(string email)
        {
            int code = GenerateConfirmationCode();

            var confirmation = new EmailConfirmation
            {
                Email = email,
                Code = code,
                ExpiryTime = DateTime.Now.AddMinutes(2)
            };

            _confirmationRepository.AddToDatabase(confirmation);
            await SendEmailAsync(email, "Your Confirmation Code", $"Your code is {code}. It is valid for 2 minutes.");
        }

        public bool VerifyCode(string email, int code)
        {
            var confirmation = _confirmationRepository.GetFromDatabaseByEmail(email); 

            if (confirmation == null || confirmation.ExpiryTime < DateTime.Now)
            {
                return false; 
            }

            return confirmation.Code == code;
        }
    }
}
