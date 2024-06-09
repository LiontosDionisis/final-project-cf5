using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services.Exceptions
{
    public class CategoryAlreadyExistsException : Exception
    {
        public CategoryAlreadyExistsException(string s) : base(s)
        {}
        
    }
}