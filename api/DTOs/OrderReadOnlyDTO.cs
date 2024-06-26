using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.DTOs
{
    public class OrderReadOnlyDTO
    {
        public int UserId {get; set;}
        public int Id { get; set; }
        public decimal Price {get; set;}
        public ICollection<Food>? Items {get; set;}
        public String Address {get; set;} = String.Empty;
    }
}