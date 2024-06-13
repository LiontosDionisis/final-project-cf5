using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class UserReadOnlyDTO
    {
        public int Id { get; set; }
        public String? Username { get; set; } 
        public String? Email {get; set;}
        public String? Password {get; set;}
        public String? UserRole {get; set;}
    }
}