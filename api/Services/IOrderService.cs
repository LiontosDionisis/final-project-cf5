using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;

namespace api.Services
{
    public interface IOrderService
    {
        Task<OrderReadOnlyDTO> GetOrderByIdAsync(int id);
        Task<List<OrderReadOnlyDTO>> GetAllOrdersAsync();
        Task<List<OrderReadOnlyDTO>> GetOrdersByUserId(int id);
        Task<OrderReadOnlyDTO> AddOrderAsync(OrderInsertDTO dto);
    }
}