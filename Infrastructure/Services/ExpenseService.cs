using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }
        // Obtener un gasto por ID
        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            var expe = await _expenseRepository.GetByIdAsync(id);
            if (expe == null)
            {
                throw new Exception("Gasto no se encontró");
            }

            return expe;
        }

        // 1- Alta
        public async Task<Expense> CreateExpenseAsync(ExpenseDTO expenseDTO, int userId, int expenseCategoryId)
        {
            // Validaciones, reglas de negocio, etc.
            if (string.IsNullOrEmpty(expenseDTO.Description))
            {
                throw new ArgumentException("La descripción de gasto no puede estar vacío.");
            }

            // Llamar al repositorio para crear la categoría
            var exp = new Expense
            {
                Amount = expenseDTO.Amount,
                Date = DateTime.UtcNow,
                Description = expenseDTO.Description,
                UserId = userId,
                ExpenseCategoryId = expenseCategoryId
            };

            return await _expenseRepository.AddAsync(exp);
        }

        // 2- Listar todas las categorias
        public async Task<List<ExpenseResponseDTO>> GetExpensesAsync()
        {
            var exp = await _expenseRepository.GetAllAsync();

            return exp.Select(x => new ExpenseResponseDTO
            {
                Id = x.Id,
                Amount = x.Amount,
                Description = x.Description,
                Date = DateTime.UtcNow,
                UserId = x.UserId,
                ExpenseCategoryId = x.ExpenseCategoryId
            }).ToList();
        }

        // 3- Modificar
        public async Task<ExpenseResponseDTO> UpdateExpenseAsync(int id, ExpenseDTO expenseDTO)
        {
            var existingExpense = await _expenseRepository.GetByIdAsync(id);
            if (existingExpense == null)
            {
                return null;
            }

            existingExpense.Amount = expenseDTO.Amount;
            existingExpense.Description = expenseDTO.Description;
            existingExpense.Date = DateTime.UtcNow;

            await _expenseRepository.UpdateAsync(existingExpense);

            return new ExpenseResponseDTO
            {
                Id = existingExpense.Id,
                Amount = existingExpense.Amount,
                Description = existingExpense.Description,
                Date = existingExpense.Date,
                UserId = existingExpense.UserId,
                ExpenseCategoryId=existingExpense.ExpenseCategoryId
            };
        }

        // Eliminar
        public async Task DeleteExpenseAsync(int id)
        {
            // Lógica de negocio antes de eliminar la categoría
            var exp = await _expenseRepository.GetByIdAsync(id);
            if (exp == null)
            {
                throw new Exception("Gasto no encontrado.");
            }

            await _expenseRepository.DeleteAsync(id);
        }
    }
}
