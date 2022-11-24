using Apple_Store_Db_Server.Models;
using Apple_Store_Db_Server.Services;
using Apple_Store_Db_Server.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Apple_Store_Db_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly IEncryptPassword _encrypted_password;
        private readonly IVerifyPassword _verify_password;
        private readonly IEmailService _sendEmail;
        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _context;

        public AuthController(ApplicationContext context, IConfiguration configuration, IVerifyPassword verify_password, IEncryptPassword encrypted_password, IEmailService sendEmail)
        {
            _sendEmail = sendEmail;
            _encrypted_password = encrypted_password;
            _verify_password = verify_password;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto ?request)
        {
            if (ModelState.IsValid)
            {
                if (request == null)
                {
                    return BadRequest();
                }

                _encrypted_password.CreatePasswordHashandSalt(request.Password, out byte[] salt, out byte[] hash);

                User user = new User
                {
                    Age = request.Age,
                    Name = request.Name,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    PasswordHash = hash,
                    PasswordSalt = salt

                };

                if (user == null)
                {
                    return BadRequest();
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _sendEmail.SendEmail(request.Email, "Apple Store",
                    $"<h1>Congrats, {request.Name} you are member of our community</h1>");
                return Ok(user);
            }
            else return BadRequest();
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(p => p.Email == request.Email);

                if (user == null)
                {
                    return NotFound();
                }
                if (!_verify_password.IsPasswordVerify(request.Password, user.PasswordSalt, user.PasswordHash))
                {
                    return NotFound();
                }

                string token = CreateToken(user);

                return Ok(new { token, user });
            }
            else return BadRequest();
        }

        [HttpDelete("removeAccount")]
        public async Task<ActionResult<User>> RemoveAccount(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            User user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [NonAction]
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(3),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
