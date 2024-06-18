using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class JwtResponse
    {
        public string? Token { get; set; }
        public string? Expires { get; set; }
        public string? Role { get; internal set; }
    }
}