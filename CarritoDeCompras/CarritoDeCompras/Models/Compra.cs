using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarritoDeCompras.Models
{
    public class Compra
    {
        [Key]
        public Guid Id { get; set; }

        //Corregir FK
        public Cliente Cliente { get; set; }

        //Corregir FK
        public Carrito Carrito { get; set; }


        public double Total { get; set; }
    }
}
