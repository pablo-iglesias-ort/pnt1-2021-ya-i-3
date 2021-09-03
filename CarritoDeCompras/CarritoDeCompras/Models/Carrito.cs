using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarritoDeCompras.Models
{
    public class Carrito
    {
        public Guid Id { get; set; }

        public string Activo { get; set; }
        
        public Cliente Cliente { get; set; }
        
        public  List<CarritoItem> Items { get; set; }

        public double Subtotal { get; set; }
    }
}
