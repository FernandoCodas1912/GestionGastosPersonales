
using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IExpenseRepository
    {
        // Obtener el id de la tabla Gastos
        Task<Expense> GetByIdAsync(int id);

        // Listar todos los gastos
        Task<List<Expense>> GetAllAsync();

        // Alta de Gastos
        Task<Expense> AddAsync(Expense expense);

        // Modificar
        Task UpdateAsync(Expense expense);

        // Eliminar
        Task DeleteAsync(int id);
    }
}
