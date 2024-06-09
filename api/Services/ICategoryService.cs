using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;

namespace api.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryReadOnlyDTO>> GetAllCategoriesAsync();
        Task<CategoryReadOnlyDTO?> GetCategoryByIdAsync(int id);
        Task<CategoryReadOnlyDTO?> GetCategoryByNameAsync(string name);
        Task<CategoryReadOnlyDTO> CreateCategoryAsync(CategoryInsertDTO dto);
        Task<CategoryReadOnlyDTO?> DeleteCategoryAsync(int id);
        Task<CategoryReadOnlyDTO?> UpdateCategoryAsync(CategoryUpdateDTO dto, int id);
    }
}