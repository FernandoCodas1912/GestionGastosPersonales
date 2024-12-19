using Core.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IExpenseService
    {
        //// Obtener el id 
        Task<Expense> GetExpenseByIdAsync(int id);

        // Alta de Categoria de Gastos
        Task<Expense> CreateExpenseAsync(ExpenseDTO expenseDTO, int userId, int expenseCategoryId);

        //// Listar todas las categorias como Lectura
        Task<List<ExpenseResponseDTO>> GetExpensesAsync();

        //// Modificar 
        Task<ExpenseResponseDTO> UpdateExpenseAsync(int id, ExpenseDTO expenseDTO);

        //// Eliminar
        Task DeleteExpenseAsync(int id);
    }
}
