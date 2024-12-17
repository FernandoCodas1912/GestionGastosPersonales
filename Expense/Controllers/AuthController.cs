using Core.DTOs;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Expense.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)

        {
            try
            {
                var user = await _userService.ValidateUserCredentialsAsync(loginDTO.Email, loginDTO.Password);

                var token = _tokenService.GenerateTokenAsync(user);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new {message = ex.Message});
            }
        }

    }
}
