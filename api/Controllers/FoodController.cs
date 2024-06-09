using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/food")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService? _service;
        public FoodController(IFoodService service)
        {
            _service = service;
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
    }
}