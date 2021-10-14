using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models
{
    public class Carrito
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Activo { get; set; }
        
        [Required]
        public Cliente Cliente { get; set; }
        
        public  List<CarritoItem> Items { get; set; }

        [Required]
        [Range(0.0, 999999999999.99)]
        public double Subtotal { get; set; }
    }
}
