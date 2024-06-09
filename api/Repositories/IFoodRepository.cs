using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace api.Repositories
{
    public interface IFoodRepository
    {
        Task<Food?> GetByIdAsync(int id);
        Task<List<Food>> GetAllAsync();
        Task<Food> GetByNameAsync(string name);
        Task<Food> AddFoodAsync(Food food);
        Task<Food?> DeleteFoodAsync(int id);
        Task<Food?> UpdateFoodAsync(Food food, int id);
    }
}