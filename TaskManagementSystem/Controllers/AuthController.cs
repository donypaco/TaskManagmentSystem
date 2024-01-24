
//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.DTO;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public AuthController( IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest();

                var token = _userService.Register(model);
                if (token != null)
                {
                    return Ok(new { Token = token });
                }
                else
                {
                    return StatusCode(500, "An error occurred while registering.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }

        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest();

                var token = _userService.Login(model);
                if (token != null)
                {
                    return Ok(new { Token = token });
                }
                else
                {
                    return StatusCode(500, "An error occurred while logging in.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }

        }
    }
}
