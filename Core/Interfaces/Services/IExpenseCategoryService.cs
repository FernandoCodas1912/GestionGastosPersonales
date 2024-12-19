
using Core.DTOs;
using Core.Entities;
using System.Globalization;

namespace Core.Interfaces.Services
{
    public interface IExpenseCategoryService
    {
        //// Obtener el id 
        Task<ExpenseCategory> GetCategoryByIdAsync(int id);

        // Alta de Categoria de Gastos
        Task<ExpenseCategory> CreateCategoryAsync(ExpenseCategoryDTO expenseCategoryDTO, int userId);

        //// Listar todas las categorias como Lectura
        Task<List<ExpenseCategoryResponseDTO>> GetCategoriesAsync();

        //// Modificar 
        Task<ExpenseCategoryResponseDTO> UpdateCategoryAsync(int id, ExpenseCategoryDTO categoryDTO);

        //// Eliminar
        Task DeleteCategoryAsync(int id);



    }
}
