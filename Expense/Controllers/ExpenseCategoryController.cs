using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System;
using System.Security.Claims;

namespace Expense.Controllers
{
    public class ExpenseCategoryController : BaseApiController
    {
        private readonly IExpenseCategoryService _expenseCategoryService;

        public ExpenseCategoryController(IExpenseCategoryService expenseCategoryService)
        {
            _expenseCategoryService = expenseCategoryService;
        }

        // Crear una nueva categoría
        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategory([FromBody] ExpenseCategoryDTO expenseCategoryDTO)
        {
            var userId = expenseCategoryDTO.UserId;  // Si es parte del DTO

            var createdCategory = await _expenseCategoryService.CreateCategoryAsync(expenseCategoryDTO, userId);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }

        // Actualizar una categoría existente
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] ExpenseCategoryDTO expenseCategoryDTO)
        {
            if (expenseCategoryDTO == null || string.IsNullOrEmpty(expenseCategoryDTO.Name))
            {
                return BadRequest("Datos de categoría inválidos.");
            }

            try
            {
                var updatedCategory = await _expenseCategoryService.UpdateCategoryAsync(id, expenseCategoryDTO);
                if (updatedCategory == null)
                {
                    return NotFound(new { message = "Categoria no encontrada." });
                }

                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al actualizar la categoria", error = ex.Message });
            }
        }

        // Eliminar una categoría por ID
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _expenseCategoryService.DeleteCategoryAsync(id);
                return NoContent(); // Si la eliminación es exitosa, retorna 204 (sin contenido)
            }
            catch (Exception)
            {
                return NotFound(new { message = "Categoría no encontrada." }); // Si no se encuentra, retorna 404
            }
        }

        // Obtener todas las categorías de gasto
        [HttpGet("List")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _expenseCategoryService.GetCategoriesAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Hubo un problema al obtener las categorias" });
            }
        }

        // Obtener una categoría por su ID
        [HttpGet("Category/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _expenseCategoryService.GetCategoryByIdAsync(id);
                return Ok(category); 
            }
            catch (Exception)
            {
                return NotFound(new { message = "Categoría no encontrada." }); // Si no se encuentra, retorna 404
            }
        }

        

        

    }
}
