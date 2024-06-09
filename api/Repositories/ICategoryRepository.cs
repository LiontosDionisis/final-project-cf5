using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByNameAsync(string name);
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> DeleteAsync(int id);
        Task<Category?> UpdateAsync(Category category, int id);
        Task<Category> AddCategoryAsync(Category category);
    }
}