using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.DTOs
{
    public class FoodUpdateDTO
    {
        public String Name { get; set; } = String.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public String? Category {get; set;}
    }
}