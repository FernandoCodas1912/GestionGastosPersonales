using Core.DTOs;
using Core.Interfaces.Repositories;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Controllers
{
    public class ExpenseCategoryController : BaseApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;

        public ExpenseCategoryController(ApplicationDbContext context, IExpenseCategoryRepository expenseCategoryRepository)
        {
            _context = context;
            _expenseCategoryRepository = expenseCategoryRepository;
        }

        // 1- Alta
        //[HttpPost("create-expenseCategory")]
        //public async Task<IActionResult> CreateExpenseCategory(ExpenseCategoryDTO expenseCategoryDTO)
        //{
        //    if(_context.ExpenseCategories.Any())
        //} 
    }
}
