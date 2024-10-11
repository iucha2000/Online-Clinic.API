using Microsoft.IdentityModel.Tokens;
using Online_Clinic.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace My_Login_App.API.Auth
{
    public interface IJwtManager
    {
        Token GetToken(Account user);
    }

    public class JwtManager : IJwtManager
    {
        private readonly IConfiguration _configuration;

        public JwtManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Token GetToken(Account user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id",user.Id.ToString(), ClaimTypes.NameIdentifier),
                    new Claim("Role",user.Role.ToString(), ClaimTypes.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
            };
            var tokenData = tokenHandler.CreateToken(tokenDescriptor);
            var token = new Token { AccessToken = tokenHandler.WriteToken(tokenData) };
            return token;
        }
    }

    public class Token
    {
        public string? AccessToken { get; set; }
    }
}
