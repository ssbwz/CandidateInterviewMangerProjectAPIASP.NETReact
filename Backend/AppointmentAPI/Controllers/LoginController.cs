using DataAccessLayer.Repositories;
using LogicLayer.Models;
using LogicLayer.Repositories;
using LogicLayer.Services.Classes;
using LogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace AppointmentAPI.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        IConfiguration _config;
        IUserRepository userRepository = new UserRepository();
        ILoginService loginService;

        public LoginController(IConfiguration config)
        {
            userRepository = new UserRepository();
            loginService = new LoginService(userRepository);
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult LoginRequest([FromBody] LoginRequest loginCredentials)
        {
            try
            {
                User requestingUser = loginService.LogInAttempt(loginCredentials);
                var token = GenerateToken(requestingUser);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }

        }

        private string GenerateToken(User user)
        {
            var securetKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            string key = securetKey.ToString();
            var credentials = new SigningCredentials(securetKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("role", user.Role.ToString()),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
