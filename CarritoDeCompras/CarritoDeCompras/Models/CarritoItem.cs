using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models
{
    public class CarritoItem
    {
        [Key]
        public Guid Id { get; set; }

        public Carrito Carrito { get; set; }

        public Producto Producto { get; set; }
        
        [Required]
        [Range(0.0, 999999999999.99)]
        public double ValorUnitario { get; set; }
        
        [Required(ErrorMessage = "Debe indicar una cantidad")]
        [Range (1, 9999, ErrorMessage = "Debe contener al menos 1 cantidad del producto")]
        public int Cantidad { get; set; }

        [Range(0.0, 999999999999.99)]
        public double ValorTotal { get; set; }
    }
}
