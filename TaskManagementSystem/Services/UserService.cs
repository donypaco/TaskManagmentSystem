﻿using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementSystem.Data;
using TaskManagementSystem.DTO;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager; 


        public UserService(ApplicationDbContext context, IConfiguration configuration, UserManager<User> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<string> Register(RegisterModel model)
        {
            try
            {
                var role = await _context.Roles.FindAsync(model.RoleId);
                if (role == null)
                {
                    return null;
                }

                User user = new User
                {
                    UserName = model.Username,
                    Password = _userManager.PasswordHasher.HashPassword(null, model.Password),
                    Email = model.Email,
                    Role = role
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return GenerateJwtToken(user);
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        public async Task<string> Login(LoginModel model)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(u => u.UserName == model.Username);
                if (user != null)
                {
                    var passwordVerificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                    var result = (passwordVerificationResult == PasswordVerificationResult.Success);

                    if (result)
                    {
                        var claims = new List<Claim>
                        {
                        new Claim(ClaimTypes.NameIdentifier, user.Id_User.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        };
                        return GenerateJwtToken(user);

                    }
                }
                return null;
            }
            catch(Exception ex)
            {
                throw;
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
            new Claim(ClaimTypes.Name, user.UserName),
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

        public async Task<List<RoleDTO>> GetRoles()
        {
            var roles = await _context.Roles.Select(r => new RoleDTO
            {
                Id = r.Id,
                RoleName = r.RoleName
            }).ToListAsync();

            return roles;
        }
        public async Task<List<Employee>> GetAvailableEmployees()
        {
            var employees = await _context.Users
                .Where(u => u.RoleId == 2)
                .Select(r => new Employee
                {
                    UserId = r.Id_User,
                    UserName = r.UserName
                }).ToListAsync();

            return employees;
        }

    }
}
