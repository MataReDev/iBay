using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace iBay.Tools
{
    public class JwtTokenTools
    {
        public static string GetEmailFromToken(string authorization)
        {
            authorization = authorization.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jsontoken = handler.ReadToken(authorization);
            var tokenS = jsontoken as JwtSecurityToken;
            var email = tokenS.Claims.First(c => c.Type == "email").Value;
            return email;
        }

        public static string GetRoleFromToken([FromHeader] string authorization)
        {
            authorization = authorization.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jsontoken = handler.ReadToken(authorization);
            var tokenS = jsontoken as JwtSecurityToken;
            var role = tokenS.Claims.First(c => c.Type == "role").Value;
            return role;
        }
    }
}
