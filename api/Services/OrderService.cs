using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;
using api.Repositories;
using AutoMapper;

namespace api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository? _repo;
        private readonly IMapper? _mapper;

        public OrderService(IOrderRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<OrderReadOnlyDTO> AddOrderAsync(OrderInsertDTO dto)
        {
            var order = new Order
            {
                Address = dto.Address,
                Price = dto.Price,
                Items = dto.Items
            };

            await _repo!.AddOrderAsync(order);
        
            var orderDto = new OrderReadOnlyDTO
            {
                Id = order.Id,
                Address = order.Address,
                Price = order.Price,
                Items = order.Items
            };

            return orderDto;
        }

        public async Task<List<OrderReadOnlyDTO>> GetAllOrdersAsync()
        {
            var orders = await _repo!.GetAllAsync();
            var ordersToReturn = _mapper!.Map<List<OrderReadOnlyDTO>>(orders);
            return ordersToReturn;
        }

        public async Task<OrderReadOnlyDTO> GetOrderByIdAsync(int id)
        {
            var order = await _repo!.GetByIdAsync(id);
            if (order == null) return null!;

            var ordersToReturn = _mapper!.Map<OrderReadOnlyDTO>(order);
            return ordersToReturn;
        }

        public async Task<List<OrderReadOnlyDTO>> GetOrdersByUserId(int id)
        {
            var orders = await _repo!.GetOrdersByUserId(id);
            var ordersToReturn = _mapper!.Map<List<OrderReadOnlyDTO>>(orders);
            return ordersToReturn;
        }
    }
}