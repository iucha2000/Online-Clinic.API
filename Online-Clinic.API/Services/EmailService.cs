using Online_Clinic.API.Interfaces;
using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Online_Clinic.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Online_Clinic.API.Exceptions;

namespace Online_Clinic.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfirmationRepository _confirmationRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly string _sendGridApiKey;
        string emailBodyTemplate = @"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <style>
                body {{ font-family: Arial, sans-serif; background: #f4f4f4; margin: 0; padding: 0; }}
                .container {{ max-width: 600px; margin: auto; background: #fff; border-radius: 5px; padding: 20px; }}
                .header {{ background: #007bff; color: #fff; text-align: center; padding: 10px; }}
                .code {{ font-size: 24px; font-weight: bold; color: #007bff; }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <div class=""header""><h1>{0}</h1></div>
                <p>Your code is <span class=""code"">{1}</span>.</p>
                <p>{2}</p>
            </div>
        </body>
        </html>";

        public EmailService(IEmailConfirmationRepository confirmationRepository, IAccountRepository accountRepository, IConfiguration configuration)
        {
            _confirmationRepository = confirmationRepository;
            _accountRepository = accountRepository;
            _sendGridApiKey = configuration["SendGridApiKey"];
        }

        public async Task SendVerificationEmailAsync(string toEmail, string subject, int code)
        {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress("imegreladze.im@gmail.com", "Online Clinic");
            var to = new EmailAddress(toEmail, "Registering user");
            string emailBody = string.Format(emailBodyTemplate, "Confirmation code", code, "It is valid for 2 minutes.");

            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, emailBody);

            await client.SendEmailAsync(msg);
        }

        public int GenerateConfirmationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }

        public async Task SendConfirmationCodeAsync(string email)
        {
            //if (!_accountRepository.EmailExists(email))
            //{
            //    throw new UserNotFoundException($"User with email:'{email}' does not exist");
            //}

            int code = GenerateConfirmationCode();

            var confirmation = new EmailConfirmation
            {
                Email = email,
                Code = code,
                ExpiryTime = DateTime.Now.AddMinutes(2)
            };

            _confirmationRepository.AddToDatabase(confirmation);
            await SendVerificationEmailAsync(email, "Email Confirmation Code", code);
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

        public async Task SendPasswordResetEmailAsync(string toEmail, string subject, string newPassword)
        {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress("imegreladze.im@gmail.com", "Online Clinic");
            var to = new EmailAddress(toEmail, "Registering user");
            string emailBody = string.Format(emailBodyTemplate, "Your new password", newPassword, "Log in with your new password");

            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, emailBody);

            await client.SendEmailAsync(msg);
        }

        public string GenerateNewPassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            Random random = new Random();
            return new string(Enumerable.Repeat(validChars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task SendNewPasswordAsync(string email)
        {
            if (!_accountRepository.EmailExists(email))
            {
                throw new UserNotFoundException($"User with email:'{email}' does not exist");
            }

            string newPassword = GenerateNewPassword();

            _accountRepository.UpdateUserPassword(email, newPassword);

            await SendPasswordResetEmailAsync(email, "Reset Password", newPassword);
        }
    }
}
