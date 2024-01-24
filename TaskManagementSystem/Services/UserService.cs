using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementSystem.Data;
using TaskManagementSystem.DTO;
using TaskManagementSystem.Services.Interfaces;

namespace AuctionAppBackend.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<string> Register(RegisterModel model)
        {
            try
            {
                var role = await _context.Roles.FindAsync(model.RoleId);
                if (role == null)
                {
                    return "Invalid RoleId";
                }

                User user = new User
                {
                    Username = model.Username,
                    Password = model.Password,
                    Email = model.Email,
                    RoleId = model.RoleId
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var token = GenerateJwtToken(user);

                return token;
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        public string Login(LoginModel model)
        {
            try
            {
                var userFound = _context.Users.SingleOrDefault(u =>
                u.Username == model.Username && u.Password == model.Password);

                if (userFound == null)
                    return null;

                var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, userFound.Id_User.ToString()),
            new Claim(ClaimTypes.Name, userFound.Username),
            };

                var token = GenerateJwtToken(userFound);

                return token;
            }
            catch(Exception ex)
            {

            }
            return null;

        }
        public string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id_User.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"])
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
