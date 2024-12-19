using Core.DTOs;
using Core.Interfaces.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Controllers
{
    public class ExpenseController : BaseApiController
    {
        private readonly IExpenseService _expenseService;
        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }
        // Crear un nuevo gasto
        [HttpPost("Create")]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseDTO expenseDTO)
        {
            var userId = expenseDTO.UserId;  
            var expenseCategoryId = expenseDTO.ExpenseCategoryId;

            var createdExpense = await _expenseService.CreateExpenseAsync(expenseDTO, userId, expenseCategoryId);
            return CreatedAtAction(nameof(GetExpenseById), new { id = createdExpense.Id }, createdExpense);
        }

        // Actualizar un gasto existente
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseDTO expenseDTO)
        {
            if (expenseDTO == null || string.IsNullOrEmpty(expenseDTO.Description))
            {
                return BadRequest("La descripción de gasto inválida.");
            }

            try
            {
                var updatedExpense = await _expenseService.UpdateExpenseAsync(id, expenseDTO);
                if (updatedExpense == null)
                {
                    return NotFound(new { message = "Gasto no encontrado." });
                }

                return Ok(updatedExpense);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al actualizar el gasto", error = ex.Message });
            }
        }

        // Eliminar un gasto por ID
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                await _expenseService.DeleteExpenseAsync(id);
                return NoContent(); // Si la eliminación es exitosa, retorna 204 (sin contenido)
            }
            catch (Exception)
            {
                return NotFound(new { message = "Gasto no encontrado." }); // Si no se encuentra, retorna 404
            }
        }

        // Obtener todas las categorías de gasto
        [HttpGet("List")]
        public async Task<IActionResult> GetExpenses()
        {
            try
            {
                var exp = await _expenseService.GetExpensesAsync();

                return Ok(exp);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Hubo un problema al obtener los gastos" });
            }
        }

        // Obtener un gasto por su ID
        [HttpGet("Expense/{id}")]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            try
            {
                var exp = await _expenseService.GetExpenseByIdAsync(id);
                return Ok(exp);
            }
            catch (Exception)
            {
                return NotFound(new { message = "Gasto no encontrado." }); // Si no se encuentra, retorna 404
            }
        }
    }
}
