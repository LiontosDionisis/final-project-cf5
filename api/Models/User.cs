using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class User
    {
        public int Id { get; set; }
        public String Username { get; set; } = String.Empty;
        public String Password { get; set; } = String.Empty;
        public String Email { get; set; } = String.Empty;
        public String UserRole { get; set; } = String.Empty;
    }
}