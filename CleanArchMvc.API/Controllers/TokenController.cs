using CleanArchMvc.API.Models;
using CleanArchMvc.Domain.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        private readonly IAuthenticate _authenticate;
        private readonly IConfiguration _configuration;

        public TokenController(IAuthenticate authenticate, IConfiguration configuration)
        {
            _authenticate = authenticate ??
                throw new ArgumentException(nameof(authenticate));

            _configuration = configuration;
        }


        [HttpPost("CreateUser")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize] 
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] LoginModel userInfo)
        {
            var result = await _authenticate.RegisterUser(userInfo.Email, userInfo.Password);

            if (result == true)
            {
                return Ok($"User {userInfo.Email} was create sucessfull");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "invalid user attempt.");
                return BadRequest(ModelState);
            }

        }


            [HttpPost("LoginUser")]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginModel userInfo)
        {
            var result = await _authenticate.Authenticate(userInfo.Email, userInfo.Password);

            if (result == true)
            {
                return GenerateToken(userInfo);
                //return Ok($"User {userInfo.Email} login sucessfull");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "invalid login attempt.");
                return BadRequest(ModelState);
            }


        }




        private UserToken GenerateToken(LoginModel userInfo)
        {

            var claims = new[]
            {
                new Claim("email", userInfo.Email),
                new Claim("meuvalor", "o que voce quiser"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            //gerar a chave privada
            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));


            //gerar a assinatura digital
            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(10);



            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
                );


            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration= expiration
            };

        }

    }
}
