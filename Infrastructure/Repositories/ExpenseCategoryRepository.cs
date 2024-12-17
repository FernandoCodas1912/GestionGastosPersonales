

using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories
{
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public ExpenseCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1- Alta
        public async Task CreateExpenseCategoryAsync(ExpenseCategoryDTO expenseCategoryDTO)
        {
            var expenseCategory = new ExpenseCategory
            {
                Name = expenseCategoryDTO.Name,
                Description = expenseCategoryDTO.Description,
            };

            await _context.ExpenseCategories.AddAsync(expenseCategory);
            await _context.SaveChangesAsync();
        }
    }
}
