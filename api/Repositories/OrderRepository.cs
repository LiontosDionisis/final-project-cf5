using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext? _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            var orderToAdd = await _context!.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return orderToAdd.Entity;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context?.Orders.ToListAsync()!;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            var order = await _context?.Orders.FirstOrDefaultAsync(x => x.Id == id)!;
            if (order == null) return null!;
            return order;
        }

        public async Task<List<Order>> GetOrdersByUserId(int id)
        {
            if (_context == null) return new List<Order>();

            return await _context.Orders
                         .Where(order => order.UserId == id)
                         .ToListAsync();
        }
    }
}