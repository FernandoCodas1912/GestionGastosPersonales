using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services;
using Infrastructure.Contexts;
using Infrastructure.Securities;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public UserController(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // 1- Alta
        [HttpPost("Create-User")]
        public async Task<IActionResult> CreateAsync([FromBody] UserDTO userDTO)
        {
            if (_context.Users.Any(x => x.Email == userDTO.Email))
            {
                return BadRequest("El correo electrónico ya está registrado");
            }

            //var user = userDTO.Adapt<User>(); // Mapster realiza el mapeo aqui
            var user = new User
            {
                FullName = userDTO.FullName,
                Email = userDTO.Email,
                Password = PasswordHelper.HashPassword(userDTO.Password),//BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
                CreationDate = DateTime.UtcNow,
                UpdateDate = null  // Se puede asignar un valor o dejarlo como null
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok( new { userId = user.Id});
        }

        // 2- Cambiar Password
        [HttpPut("{userId}/change-password")]
        public async Task<IActionResult> ChangePassword(int userId, [FromBody] ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                await _userService.ChangePasswordAsync(userId, changePasswordDTO.NewPassword);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 3- Soft Delete y Bloqueo
        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateUserAsync(int userId, [FromBody] UpdateUserDTO updateUserDTO)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(userId, updateUserDTO);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/block")]
        public async Task<IActionResult> BlockUser(int id)
        {
            await _userService.BlockUserAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/unblock")]
        public async Task<IActionResult> UnblockUser(int id)
        {
            try
            {
                var user = await _userService.UnblockUserAsync(id);
                return Ok( new { message = "User unblocked successfully", userId = user.Id});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}/soft-delete")]
        public async Task<IActionResult> SofDeleteUser(int id)
        {
            await _userService.SoftDeleteUserAsync(id);
            return NoContent();
        }

    }
}