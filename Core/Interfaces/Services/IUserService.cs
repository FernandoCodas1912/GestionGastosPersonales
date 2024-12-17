
using Core.Entities;
using Core.DTOs;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        // 2- Cambiar password
        Task ChangePasswordAsync(int userId, string newPassword);
        
        // 3- Sof delete y bloqueo
        Task<UserDTO> UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO);
        Task<User>BlockUserAsync(int id);
        Task<User> UnblockUserAsync(int id);
        Task<User> SoftDeleteUserAsync(int id);

        // 4 - Ingresar el usuario con Token
        Task<User> ValidateUserCredentialsAsync(string email, string password);
    }
}
