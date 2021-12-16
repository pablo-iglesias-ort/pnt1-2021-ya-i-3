using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarritoDeCompras.Models
{
    public class Compra
    {
        [Key]
        [Display(Name = "Número de Compra")]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Cliente))]
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [ForeignKey(nameof(Carrito))]
        public Guid CarritoId { get; set; }
        public Carrito Carrito { get; set; }

        [ForeignKey(nameof(Sucursal))]
        public Guid SucursalId { get; set; }

        public Sucursal Sucursal { get; set; }


        public double Total { get; set; }
    }
}
