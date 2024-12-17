
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Infrastructure.Contexts;
using Infrastructure.Securities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using SQLitePCL;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {   
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;

        public UserService(IUserRepository userRepository, ITokenService tokenService, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _context = context;
        }

        // 2- Cambiar Password
        public async Task ChangePasswordAsync(int userId, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            if (newPassword.Length < 8)
                throw new Exception("La nueva contaseña debe tener al menos 8 caracteres.");

            user.Password = PasswordHelper.HashPassword(newPassword);
            await _userRepository.UpdatePassword(user);
        }

        // 3- Soft Delete y Bloqueo
        public async Task<UserDTO> UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted);

            if (user == null)
            {
                throw new Exception("User Not Found");
            }

            user.FullName = updateUserDTO.Name;
            user.Email = updateUserDTO.Email;
            

            user.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var userDTO = new UserDTO
            {
                FullName = user.FullName,
                Email = user.Email,
            };
            return userDTO;
           
        }

        public async Task<User>BlockUserAsync(int id)
        {
            var user =  await _context.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (user == null)
                throw new Exception("User not found.");

            user.IsBlocked = true;
            await _context.SaveChangesAsync();

            return user;
            
        }
        
        public async Task<User> UnblockUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (user == null)
            {
                throw new Exception($"User with ID {id} not found or has been deleted.");
            }
                

            user.IsBlocked = false;
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User>SoftDeleteUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (user == null)
                throw new Exception("User not found.");

            user.IsDeleted = true;
            user.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return user;
        }

        // 4- Generar un token para ingresar un usuario
        public async Task<User> ValidateUserCredentialsAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            if (!VerifyPassword(password, user.Password))
            {
                throw new Exception("Contraseña incorrecta.");
            }

            return user;

        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
            return result == PasswordVerificationResult.Success;

        }

    }
}
