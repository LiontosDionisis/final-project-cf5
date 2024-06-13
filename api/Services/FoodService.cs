using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;
using api.Data;
using api.DTOs;
using api.Models;
using api.Repositories;
using api.Services.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace api.Services
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository? _foodRepo;
        private readonly IMapper? _mapper;
        private readonly AppDbContext _context;
        private readonly ICategoryService _catService;
        public FoodService(IFoodRepository foodRepo, IMapper mapper, ICategoryService catService, AppDbContext context)
        {
            _foodRepo = foodRepo;
            _mapper = mapper;
            _catService = catService;
            _context = context;
        }

        public async Task<FoodReadOnlyDTO> AddFoodAsync(FoodInsertDTO dto)
        {
            var existingFood = await _foodRepo!.GetByNameAsync(dto.Name);
            if (existingFood != null)
            {
                throw new FoodAlreadyExistsException("Food already exists with name " + dto.Name);
            }

            var existingCategory = await _catService!.GetCategoryByNameAsync(dto.Category!);
            if (existingCategory == null)
            {
                throw new ArgumentException("Category not found");
            }

            var existingCategoryEntity = await _context.Categories.FirstOrDefaultAsync(c => c.Name == existingCategory.Name);
            if (existingCategoryEntity == null)
            {
                return null!;
            }

            var foodEntity = new Food
            {
                Name = dto.Name,
                Price = dto.Price,
                Category = existingCategoryEntity
            };

            await _foodRepo!.AddFoodAsync(foodEntity);

            var foodDto = new FoodReadOnlyDTO
            {
                Id = foodEntity.Id,
                Name = foodEntity.Name,
                Price = foodEntity.Price,
                Category = _mapper!.Map<Category>(dto.Category)
            };

            return foodDto;
        }

        // public async Task<FoodReadOnlyDTO> AddFoodAsync(FoodInsertDTO dto)
        // {
        //     var existingFood = await _foodRepo!.GetByNameAsync(dto.Name);
        //     if (existingFood != null)
        //     {
        //         throw new FoodAlreadyExistsException("Food exists " + existingFood.Name);
        //     }

        //     // var existingCategory = await _catService.GetCategoryByNameAsync(dto.Category!);
        //     // if (existingCategory == null)
        //     // {
        //     //     throw new ArgumentException("Category not found");
        //     // }

        //     var food = new Food 
        //     {
        //         Name = dto.Name,
        //         Price = dto.Price,
        //         Category = _mapper!.Map<Category>(category)
        //     };

        //     await _foodRepo!.AddFoodAsync(food);
        //     return _mapper!.Map<FoodReadOnlyDTO>(food);
        // }


        public async Task<FoodReadOnlyDTO?> DeleteFoodAsync(int id)
        {
            var food = await _foodRepo!.GetByIdAsync(id);
            if (food == null) return null!;

            await _foodRepo!.DeleteFoodAsync(food.Id);
            var deletedFood = _mapper!.Map<FoodReadOnlyDTO>(food);
            return deletedFood;
        }

        public async Task<List<FoodReadOnlyDTO>> GetAllFoodAsync()
        {
            var foods = await _foodRepo!.GetAllAsync();
            var foodToReturn = _mapper!.Map<List<FoodReadOnlyDTO>>(foods);
            return foodToReturn;
        }

        public async Task<FoodReadOnlyDTO?> GetFoodByIdAsync(int id)
        {
            var food = await _foodRepo!.GetByIdAsync(id);
            if (food == null) return null!;

            var foodToReturn = _mapper!.Map<FoodReadOnlyDTO>(food);
            return foodToReturn;
        }

        public async Task<FoodReadOnlyDTO?> GetFoodByNameAsync(string name)
        {
            var food = await _foodRepo!.GetByNameAsync(name);
            if (food == null) return null!;

            var foodToReturn = _mapper!.Map<FoodReadOnlyDTO>(food);
            return foodToReturn;
        }

        public async Task<FoodReadOnlyDTO> UpdateFoodAsync(FoodUpdateDTO dto, int id)
        {
            try
            {
                var food = await _foodRepo!.GetByIdAsync(id);
                if (food == null) return null!;

                if (dto.Name != null) food.Name = dto.Name;
                if (dto?.Price != null) food.Price = dto.Price;
                var catName = await _catService.GetCategoryByNameAsync(dto!.Category!.Name);
                var catId = catName!.Id;
                if (dto?.Category != null) food.Category = dto.Category;
                    
                await _foodRepo.UpdateFoodAsync(food, food.Id);
                _context.Entry(food).Reload();
                var updatedFood = _mapper!.Map<FoodReadOnlyDTO>(food);
                return updatedFood; 
            }
            catch (Exception)
            {
                return null!;
            }
        }

    }
}