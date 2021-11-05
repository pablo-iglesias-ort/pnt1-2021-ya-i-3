using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models
{
    public class Carrito
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Boolean Activo { get; set; }
        

        [ForeignKey(nameof(Cliente))]
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }



        [Required]
        [Range(0.0, 999999999999.99)]
        public double Subtotal { get; set; }

        public IEnumerable<CarritoItem> Items { get; set; }
    }
}
