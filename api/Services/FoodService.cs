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
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository? _foodRepo;
        private readonly IMapper? _mapper;
        public FoodService(IFoodRepository foodRepo, IMapper mapper)
        {
            _foodRepo = foodRepo;
            _mapper = mapper;
        }
        public async Task<FoodReadOnlyDTO> AddFoodAsync(FoodInsertDTO dto)
        {
            var food = ExctractFood(dto);
            Food? existingFood = await _foodRepo!.GetByNameAsync(food.Name);
            if (existingFood != null)
            {
                throw new FoodAlreadyExistsException("Food exists " + existingFood.Name);
            }

            await _foodRepo!.AddFoodAsync(food);
            return _mapper!.Map<FoodReadOnlyDTO>(food);
        }

        public async Task<FoodReadOnlyDTO?> DeleteFoodAsync(int id)
        {
            var food = await _foodRepo!.GetByIdAsync(id);
            if (food == null) return null!;

            _foodRepo?.DeleteFoodAsync(food.Id);
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
            var food = await _foodRepo!.GetByNameAsync(dto.Name!);
            if (food == null) return null!;

            food.Name = dto.Name!;
            food.Price = dto.Price!;
            food.Category = dto.Category!;

            await _foodRepo.UpdateFoodAsync(food, food.Id);
            var updatedFood = _mapper!.Map<FoodReadOnlyDTO>(food);
            return updatedFood;
        }

        private Food ExctractFood(FoodInsertDTO dto)
        {
            return new Food()
            {
                Name = dto.Name!,
                Price = dto.Price!,
                Category = dto.Category
            };
        }
    }
}