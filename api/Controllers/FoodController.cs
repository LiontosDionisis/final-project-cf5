using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;
using api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/food")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService? _service;
        private readonly IMapper _mapper;
        public FoodController(IFoodService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllFoodAsync()
        {
            var foods = await _service!.GetAllFoodAsync();
            if (foods == null) return NotFound();
            return Ok(foods);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodByIdAsync([FromRoute] int id)
        {
            var food = await _service!.GetFoodByIdAsync(id);
            if (food == null) return NotFound();
            return Ok(food);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetFoodByNameAsync([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name)) return BadRequest("Name is required");

            var food = await _service!.GetFoodByNameAsync(name);
            if (food == null) return NotFound();
            return Ok(food);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFoodAsync([FromBody] FoodInsertDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var createdFood = await _service!.AddFoodAsync(dto);
                return Ok(createdFood);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new {message = e.Message});
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodByIdAsync([FromRoute] int id)
        {
            var foodToDelete = await _service!.GetFoodByIdAsync(id);
            if (foodToDelete == null) return NotFound();

            await _service.DeleteFoodAsync(id);
            return Ok(foodToDelete);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFoodAsync(int id, [FromBody] FoodUpdateDTO dto)
        {
            try
            {
                var existingFood = await _service!.GetFoodByIdAsync(id);
                if (existingFood == null)
                {
                    return NotFound();
                }

                if (dto.Name != null) existingFood.Name = dto.Name;
                if (dto?.Price != null) existingFood.Price = dto.Price;
                // var cat = _mapper.Map<Category>(dto?.Category);
                if (dto?.Category != null) 
                {
                    var category = _mapper.Map<Category>(dto.Category);
                    existingFood.Category = category;
                }
                var foodUpdated = _mapper.Map<FoodUpdateDTO>(existingFood);
                await _service.UpdateFoodAsync(foodUpdated, id);
                return Ok(foodUpdated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error while updating food", error = ex.Message });
            }
        }
    }
}