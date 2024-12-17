using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Contexts;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context; 
        }

        // 1- Alta
        public async Task CreateAsync(UserDTO userDTO)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

            var user = new User
            {
                FullName = userDTO.FullName,
                Password = hashedPassword,
                Email = userDTO.Email,
                CreationDate = DateTime.UtcNow,
                UpdateDate = null
            };

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
        }

        // 2- Cambiar Password
        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task UpdatePassword(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        // 3- Soft Delete y Bloqueo
        public async Task SoftDeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.UpdateDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task BlockUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsBlocked = true;
                await _context.SaveChangesAsync();
            }
        }

        // 4- Generar un token para ingresar el usuario
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);
        }
    }
}
