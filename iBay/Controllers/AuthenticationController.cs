using iBay.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace iBay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly Context _context;
        private readonly IConfiguration _config;

        /// <summary>
        /// Constructeur product
        /// </summary>
        /// <param name="context"></param>
        public AuthenticationController(Context context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Provides login system
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Return a login token</returns>
        /// <response code="401">Email or Password is incorrect</response>
        /// <response code="200">Return a token</response>
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string password)
        {
            String hashPassword = Password.hashPassword(password);

            var dbUser = _context.User.Where(u => u.Email == email.ToLower() && u.Password == hashPassword).FirstOrDefault();
            if (dbUser == null) return Unauthorized("Email or Password is incorrect");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, dbUser.Email),
                new Claim(ClaimTypes.Role, dbUser.Role),
            };

            string token = GenerateTokenString(_config["Jwt:Key"],claims);
            return Ok(token);
        }

        [HttpGet]
        public IActionResult TestTOKEN(string tokenComplet)
        {
            if (tokenComplet != null && tokenComplet.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                var token = tokenComplet.Substring("Bearer ".Length).Trim();
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                return Ok(jwt.Claims.First(c => c.Type == "email").Value);
            }
            return Ok("pas de token");
        }

        [NonAction]
        public string GenerateTokenString(string secret, List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
    }
}
