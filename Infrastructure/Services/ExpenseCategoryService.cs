using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;  // Cambia a la interfaz
        public ExpenseCategoryService(IExpenseCategoryRepository expenseCategoryRepository)
        {
            _expenseCategoryRepository = expenseCategoryRepository;  // Inyección de la interfaz
        }

        // Obtener una categoria por ID
        public async Task<ExpenseCategory> GetCategoryByIdAsync(int id)
        {
            var category = await _expenseCategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Categoria no encuentra");
            }

            return category;
        }

        // 1- Alta
        public async Task<ExpenseCategory> CreateCategoryAsync(ExpenseCategoryDTO expenseCategoryDTO, int userId)
        {
            // Validaciones, reglas de negocio, etc.
            if (string.IsNullOrEmpty(expenseCategoryDTO.Name))
            {
                throw new ArgumentException("El nombre de categoria no puede estar vacío.");
            }

            // Llamar al repositorio para crear la categoría
            var category = new ExpenseCategory
            {
                Name = expenseCategoryDTO.Name,
                Description = expenseCategoryDTO.Description,
                UserId = userId
            };

            return await _expenseCategoryRepository.AddAsync(category);
        }

        // 2- Listar todas las categorias
        public async Task<List<ExpenseCategoryResponseDTO>> GetCategoriesAsync()
        {
            var categories = await _expenseCategoryRepository.GetAllAsync();

            return categories.Select(x => new ExpenseCategoryResponseDTO
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                UserId = x.UserId
            }).ToList();
        }
        
        // 3- Modificar
        public async Task<ExpenseCategoryResponseDTO> UpdateCategoryAsync(int id, ExpenseCategoryDTO expenseCategoryDTO)
        {
            var existingCategory = await _expenseCategoryRepository.GetByIdAsync(id);
            if(existingCategory == null)
            {
                return null;
            }

            existingCategory.Name = expenseCategoryDTO.Name;
            existingCategory.Description = expenseCategoryDTO.Description;

            await _expenseCategoryRepository.UpdateAsync(existingCategory);

            return new ExpenseCategoryResponseDTO
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                Description = existingCategory.Description,
                UserId = existingCategory.UserId
            };
        }

        // Eliminar
        public async Task DeleteCategoryAsync(int id)
        {
            // Lógica de negocio antes de eliminar la categoría
            var category = await _expenseCategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found.");
            }

            await _expenseCategoryRepository.DeleteAsync(id);
        }

    }
}
