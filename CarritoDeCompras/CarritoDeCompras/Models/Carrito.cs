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

        public string Activo { get; set; }
        
        [Required]
        public Cliente Cliente { get; set; }
        
        public  List<CarritoItem> Items { get; set; }

        public double Subtotal { get; set; }
    }
}
