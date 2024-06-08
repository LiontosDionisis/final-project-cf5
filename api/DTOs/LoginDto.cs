using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class LoginDto
    {
        public String? Username { get; set; }
        public String? Password { get; set; }
    }
}