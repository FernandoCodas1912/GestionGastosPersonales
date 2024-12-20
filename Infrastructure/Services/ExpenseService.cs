using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                Date = expenseDTO.Date ?? DateTime.UtcNow,
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
            existingExpense.Date = expenseDTO.Date ?? DateTime.UtcNow;

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

        // Obtener los gastos: paginas, tamaño, palabra, categoriaId
        public async Task<PagedExpenseResponseDTO> GetFilteredExpenseAsync(int page, int pageSize, string? search, int? expenseCategoryId, string? orderBy, string? orderDirection)
        {
            // Para evitar los valores negativos
            if(page < 0)
            {
                throw new ArgumentException("El número de la página debe ser mayor que 0.");
            }
            if(pageSize < 0)
            {
                throw new ArgumentException("El tamñao de la página debe ser mayor que 0.");
            }

            // Calcular el salto de la paginación
            int skip = (page - 1) * pageSize;

            var exp = await _expenseRepository.GetAllAsync(); //AsQueryable -> es un método que permite convertir la consulta en una instancia de IQueryable


            // Filtrar por palabra clave en la descripción
            if (!string.IsNullOrEmpty(search))
            {
                search = RemoveAccents(search.ToLower()); // Eliminar los acentos y pasar a minúsculas
                exp = exp.Where(x => x.Description.ToLower().Contains(search)).ToList();
            }

            // Filtro por categoria
            if (expenseCategoryId.HasValue)
            {
                exp = exp.Where(x => x.ExpenseCategoryId == expenseCategoryId.Value).ToList();
            }

            /// Ordenamiento ///
            if (!string.IsNullOrEmpty(orderBy))
            {
                if(orderBy.ToLower() == "amount")
                {
                    exp = orderDirection?.ToLower() == "desc" ? exp.OrderByDescending(x => x.Amount).ToList() : exp.OrderBy(x => x.Amount).ToList();
                }
                else if(orderBy.ToLower() == "date")
                {
                    exp = orderDirection?.ToLower() == "desc" ? exp.OrderByDescending(x => x.Date).ToList() : exp.OrderBy(x => x.Date).ToList();

                }
            }

            // Obtener total de elementos(para paginación)
            var totalCount = exp.Count();

            // Calcular el total de p{aginas
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Aplicar paginación
            var pagedList = exp.Skip(skip).Take(pageSize).ToList();

            // Mapear a ExpenseResponseDTO
            var result = pagedList.Select(x => new ExpenseResponseDTO
            {
                Id = x.Id,
                Amount = x.Amount,
                Description = x.Description,
                Date = x.Date,
                UserId = x.UserId,
                ExpenseCategoryId = x.ExpenseCategoryId
            }).ToList();

            return new PagedExpenseResponseDTO
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Items = result
            };
        }

        // Auxiliar para eliminar acentos
        private string RemoveAccents(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
