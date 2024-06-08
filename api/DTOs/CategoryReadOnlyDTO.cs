using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class CategoryReadOnlyDTO
    {
        public int Id { get; set; }    
        public String? Name { get; set; }
    }
}