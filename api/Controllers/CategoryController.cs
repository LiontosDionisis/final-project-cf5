using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Models;
using api.Repositories;
using api.Services;
using api.Services.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService? _service;
        
        public CategoryController(ICategoryService service)
        {
            _service = service;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var cats = await _service!.GetAllCategoriesAsync();
            if (cats == null) return NotFound();
            return Ok(cats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var cat = await _service!.GetCategoryByIdAsync(id);
            if (cat == null) return NotFound();
            return Ok(cat);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetByNameAsync([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name is required");
            }

            var cat = await _service!.GetCategoryByNameAsync(name);
            if (cat == null) return NotFound($"Category with name '{name} was not found'");
            return Ok(cat);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryInsertDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var createdCat = await _service!.CreateCategoryAsync(dto);
                return Ok(createdCat);
            }
            catch (CategoryAlreadyExistsException e)
            {
                return Conflict(new {message = e.Message});
            }
            catch (Exception)
            {
                return StatusCode(500, new {message = "An error occured while trying to create the category"});
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var category = await _service!.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            await _service.DeleteCategoryAsync(id);
            return Ok(category);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] CategoryUpdateDTO dto, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

             try
            {
                var existingCategory = await _service!.GetCategoryByIdAsync(id);
                if (existingCategory == null) return NotFound();

                existingCategory.Name = dto.Name;
                
                var updatedCat = await _service.UpdateCategoryAsync(dto, id);

                if (updatedCat != null) return Ok(updatedCat);
                return StatusCode(500, new {message = "Error while updating category."});
            }
            catch (Exception)
            {   
                return StatusCode(500, new { message = "Error occurred while updating category" });
            }
        }
    }
}