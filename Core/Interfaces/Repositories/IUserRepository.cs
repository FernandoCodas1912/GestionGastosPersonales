using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        // 1- Alta 
        Task CreateAsync(UserDTO userDTO);

        // 2- Cambiar Password
        Task<User> GetByIdAsync(int id);
        Task UpdatePassword(User user);

        // 3- Soft Delete y Bloqueo
        Task SoftDeleteAsync(int id);
        Task BlockUserAsync(int id);

        Task<User> GetByEmailAsync(string email);
    }
}
