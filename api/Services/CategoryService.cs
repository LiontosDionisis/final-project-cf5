using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;
using api.Repositories;
using api.Services.Exceptions;
using AutoMapper;

namespace api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository? _catRepo;
        private readonly IMapper? _mapper;
        public CategoryService(ICategoryRepository catRepo, IMapper mapper)
        {
            _catRepo = catRepo;
            _mapper = mapper;
        }
        public async Task<CategoryReadOnlyDTO> CreateCategoryAsync(CategoryInsertDTO dto)
        {
            var existingCategory = await _catRepo?.GetByNameAsync(dto.Name!)!;
            if (existingCategory != null)
            {
                throw new ArgumentException("Category already exists");
            }
            
            var categoryEntity = new Category {Name = dto.Name!};

            _catRepo?.AddCategoryAsync(categoryEntity);

            var catDto = new CategoryReadOnlyDTO
            {
                Id = categoryEntity.Id,
                Name = categoryEntity.Name
            };

            return catDto;
            // var cat = ExtractCategory(dto);
            // Category? existingCategory = await _catRepo!.GetByNameAsync(cat.Name);
            // if (existingCategory != null)
            // {
            //     throw new CategoryAlreadyExistsException("Category exists " + existingCategory.Name);
            // }

            // await _catRepo!.AddCategoryAsync(cat);
            // return _mapper!.Map<CategoryReadOnlyDTO>(cat);
        }

        public async Task<CategoryReadOnlyDTO?> DeleteCategoryAsync(int id)
        {
            var cat = await _catRepo!.GetByIdAsync(id);
            if (cat == null) return null;

            await _catRepo!.DeleteAsync(cat.Id);
            var deletedCat = _mapper!.Map<CategoryReadOnlyDTO>(cat);
            return deletedCat;
        }

        public async Task<List<CategoryReadOnlyDTO>> GetAllCategoriesAsync()
        {
            var cats = await _catRepo!.GetAllAsync();
            var catsToReturn = _mapper!.Map<List<CategoryReadOnlyDTO>>(cats);
            return catsToReturn;
        }

        public async Task<CategoryReadOnlyDTO?> GetCategoryByIdAsync(int id)
        {
            var cat = await _catRepo!.GetByIdAsync(id);
            if (cat == null) return null!;

            var catToReturn = _mapper!.Map<CategoryReadOnlyDTO>(cat);
            return catToReturn;
        }

        public async Task<CategoryReadOnlyDTO?> GetCategoryByNameAsync(string name)
        {
            var cat = await _catRepo!.GetByNameAsync(name);
            if (cat == null) return null!;

            var catToReturn = _mapper!.Map<CategoryReadOnlyDTO>(cat);
            return catToReturn;
        }

        public async Task<CategoryReadOnlyDTO?> UpdateCategoryAsync(CategoryUpdateDTO dto, int id)
        {
            var cat = await _catRepo!.GetByIdAsync(id);
            if (cat == null) return null!;

            cat.Name = dto.Name!;

            await _catRepo.UpdateAsync(cat, cat.Id);

            var updatedCat = _mapper!.Map<CategoryReadOnlyDTO>(cat);
            return updatedCat;
        }

        private Category ExtractCategory(CategoryInsertDTO dto)
        {
            return new Category()
            {
                Name = dto.Name!
            };
        }
    }
}