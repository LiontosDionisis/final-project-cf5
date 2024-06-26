using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;
using AutoMapper;

namespace api.Mapper
{
    public class DTOMapper : Profile
    {
        public DTOMapper()
        {
            CreateMap<User, UserInsertDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();
            CreateMap<User, UserReadOnlyDTO>().ReverseMap();
            CreateMap<UserReadOnlyDTO, UserUpdateDTO>().ReverseMap();
            CreateMap<Category, CategoryInsertDTO>().ReverseMap();
            CreateMap<Category, CategoryUpdateDTO>().ReverseMap();
            CreateMap<Category, CategoryReadOnlyDTO>().ReverseMap();
            CreateMap<string, Category>()
            .ConvertUsing(source => new Category { Name = source });
            CreateMap<FoodReadOnlyDTO, FoodUpdateDTO>().ReverseMap();
            CreateMap<Food, FoodReadOnlyDTO>().ReverseMap();
            CreateMap<Food, FoodInsertDTO>().ReverseMap();
            CreateMap<Food, FoodUpdateDTO>().ReverseMap();
            CreateMap<Order, OrderInsertDTO>().ReverseMap();
            CreateMap<Order, OrderReadOnlyDTO>().ReverseMap();
        }
        
    }
}