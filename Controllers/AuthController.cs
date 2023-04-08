using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReckTec.NetCore.LiYang.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpPost("Token")]
        public IActionResult Token(string username, string password)
        {
            if (username == "admin" && password == "1")
            {
                var token = GenerateToken();

                return Ok(new { Token = token });
            }

            return Ok(new { ErrorCode = 999, Message = "用户名或密码不正确" });
        }

        private string GenerateToken()
        {
            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role,"admin"),
            new Claim(ClaimTypes.Role, "admin"),
            new Claim("UserId", "1111"),
            new Claim("UserName", "admin"),
            new Claim("Phone", "111"),
           };
            DateTime notBefore = DateTime.Now;
            DateTime expires = DateTime.Now.AddDays(1);
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey")));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(null, null, claims, notBefore, expires, signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);



        }
    }
}
