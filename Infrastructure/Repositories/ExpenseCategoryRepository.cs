

using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Infrastructure.Repositories
{
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public ExpenseCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener el Id
        public async Task<ExpenseCategory> GetByIdAsync(int id)
        {
            return await _context.ExpenseCategories.FirstOrDefaultAsync(x => x.Id == id);
        }
        
        // Listar todas las categorias
        public async Task<List<ExpenseCategory>> GetAllAsync()
        {
            return await _context.ExpenseCategories.ToListAsync();
        }

        //// Alta
        public async Task<ExpenseCategory> AddAsync(ExpenseCategory expenseCategory)
        {
            _context.ExpenseCategories.Add(expenseCategory);
            await _context.SaveChangesAsync();
            return expenseCategory;
        }

        //Modificar
        public async Task UpdateAsync(ExpenseCategory expenseCategory) 
        {
            _context.ExpenseCategories.Update(expenseCategory);
            await _context.SaveChangesAsync();
            //return expenseCategory;
        }

        // Eliminar
        public async Task DeleteAsync(int id)
        {
             var category = await GetByIdAsync(id);
            if (category != null)
            {
                _context.ExpenseCategories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
