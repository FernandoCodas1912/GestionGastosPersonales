
using Core.DTOs;

namespace Core.Interfaces.Repositories
{
    public interface IExpenseCategoryRepository
    {
        // Alta de Categoria de Gastos
        Task CreateExpenseCategoryAsync(ExpenseCategoryDTO expenseCategoryDTO);
        
        // Obtener el id de la tabla Categoria de Gastos
        //Task GetByIdAsync(int id);




    }
}
