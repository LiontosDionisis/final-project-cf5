using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price {get; set;}
        [Required]
        public int UserId {get; set;}
        [Required]
        public ICollection<Food>? Items {get; set;}
        [Required]
        public String Address {get; set;} = String.Empty;
    }
}