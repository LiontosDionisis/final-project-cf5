using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;

namespace api.Services
{
    public interface IFoodService
    {
        Task<List<FoodReadOnlyDTO>> GetAllFoodAsync();
        Task<FoodReadOnlyDTO?> GetFoodByIdAsync(int id);
        Task<FoodReadOnlyDTO?> GetFoodByNameAsync(string name);
        Task<FoodReadOnlyDTO> AddFoodAsync(FoodInsertDTO dto);
        Task<FoodReadOnlyDTO?> DeleteFoodAsync(int id);
        Task<FoodReadOnlyDTO> UpdateFoodAsync(FoodUpdateDTO dto, int id);

    }
}