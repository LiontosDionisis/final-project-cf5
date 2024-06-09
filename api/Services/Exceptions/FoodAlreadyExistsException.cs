using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services.Exceptions
{
    public class FoodAlreadyExistsException : Exception
    {
        public FoodAlreadyExistsException(string s) : base(s)
        {
            
        }
    }
}