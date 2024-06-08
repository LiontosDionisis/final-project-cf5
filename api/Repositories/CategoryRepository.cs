using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext? _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Category> AddCategoryAsync(Category category)
        {
            var categoryToAdd = await _context!.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return categoryToAdd.Entity;
        }

        public async Task<Category?> DeleteAsync(int id)
        {
            var categoryToDelete = await _context?.Categories.FirstOrDefaultAsync(x => x.Id == id)!;
            if (categoryToDelete == null) return null;
            _context.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
            return categoryToDelete;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context?.Categories.ToListAsync()!;
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            var category = await _context?.Categories.FirstOrDefaultAsync(x => x.Id == id)!;
            if (category == null) return null;
            return category;
        }

        public async Task<Category?> UpdateAsync(Category category, int id)
        {
            var existingCategory = await _context!.Categories.FindAsync(id);
            if (existingCategory == null) return null;
            
            _context.Entry(existingCategory).CurrentValues.SetValues(category);
            await _context.SaveChangesAsync();
            return existingCategory;
        }
    }
}