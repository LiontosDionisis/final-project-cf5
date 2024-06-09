using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class FoodRepository : IFoodRepository
    {

        private readonly AppDbContext? _context;

        public FoodRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Food> AddFoodAsync(Food food)
        {
            var foodToAdd = await _context!.Foods.AddAsync(food);
            await _context.SaveChangesAsync();

            return foodToAdd.Entity;
        }

        public async Task<Food?> DeleteFoodAsync(int id)
        {
            var foodToDelete = await _context?.Foods.FirstOrDefaultAsync(x => x.Id == id)!;
            if (foodToDelete == null) return null;

            _context.Foods.Remove(foodToDelete);
            await _context.SaveChangesAsync();
            return foodToDelete;
        }

        public async Task<List<Food>> GetAllAsync()
        {
            return await _context?.Foods.ToListAsync()!;
        }

        public async Task<Food> GetByNameAsync(string name)
        {
            var food = await _context!.Foods.FirstOrDefaultAsync(x => x.Name == name)!;
            if (food == null) return null!;

            return food;
        }

        public async Task<Food?> GetFoodByIdAsync(int id)
        {
            var food = await _context?.Foods.FirstOrDefaultAsync(x => x.Id == id)!;
            if (food == null) return null;
            return food;
        }

        public async Task<Food?> UpdateFoodAsync(Food food, int id)
        {
            var foodToUpdate = await _context?.Foods.FirstOrDefaultAsync(x => x.Id == id)!;
            if (foodToUpdate == null) return null;

            _context.Entry(foodToUpdate).CurrentValues.SetValues(food);
            await _context.SaveChangesAsync();
            return foodToUpdate;
        }
    }
}