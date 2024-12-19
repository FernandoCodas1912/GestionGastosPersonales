
using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IExpenseCategoryRepository
    {

        // Obtener el id de la tabla Categoria de Gastos
        Task<ExpenseCategory> GetByIdAsync(int id);

        // Listar todas las categorias
        Task<List<ExpenseCategory>> GetAllAsync();

        // Alta de Categoria de Gastos
        Task<ExpenseCategory> AddAsync(ExpenseCategory expenseCategory);

        // Modificar
        Task UpdateAsync(ExpenseCategory expenseCategory);

        // Eliminar
        Task DeleteAsync(int id);




    }
}
