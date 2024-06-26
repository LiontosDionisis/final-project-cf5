using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService? _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var orders = await _service!.GetAllOrdersAsync();
            if (orders == null) return NotFound();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var order = await _service!.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserIdAsync([FromRoute] int id)
        {
            var orders = await _service!.GetOrdersByUserId(id);
            if (orders == null) return NotFound();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderInsertDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest();

             if (dto.Items == null || dto.Items.Count == 0)
            {
                return BadRequest("Order must contain at least one item.");
            }

            try
            {
                var createdOrder = await _service!.AddOrderAsync(dto);
                return Ok(createdOrder);
            }
            catch (Exception)
            {
                return StatusCode(500, new {message = "An error occured while trying to create the order"});
            }
        }
    }
}